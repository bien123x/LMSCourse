using AutoMapper;
using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.PaymentDto;
using LMSCourse.DTOs.ZaloPay;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using ZaloPay.Helper;
using ZaloPay.Helper.Crypto;

namespace LMSCourse.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly IEnrollmentRepository _enrollRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly ZaloPayOptions _options;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;


        public PaymentService(IPaymentRepository repo, IOptions<ZaloPayOptions> options, IMapper mapper, IEnrollmentRepository enrollRepo, ICourseRepository courseRepository, IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _repo = repo;
            _options = options.Value;
            _mapper = mapper;
            _enrollRepo = enrollRepo;
            _courseRepo = courseRepository;
            _scopeFactory = scopeFactory;
            _config = config;
        }

        public async Task<Dictionary<string, object>> CreateOrderAsync(PaymentDto dto)
        {
            var transid = new Random().Next(1000000);
            var app_trans_id = DateTime.Now.ToString("yyMMdd") + "_" + transid;
            var embeddata = new { redirecturl = $"{_config["AppUrls:Frontend"]}/student/payment-result" };
            var items = dto.CourseDtos.Select(c => new
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Price = c.Price,
            }).ToList();

            var param = new Dictionary<string, string>();

            param.Add("app_id", _options.AppId);
            param.Add("app_user", dto.UserId.ToString());
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("amount", dto.Amount.ToString());
            param.Add("app_trans_id", app_trans_id);
            param.Add("callback_url", $"{_config["AppUrls:Public"]}/Payment/callback");

            param.Add("embed_data", JsonConvert.SerializeObject(embeddata));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", $"Thanh toán đơn hàng {transid}");
            param.Add("bank_code", dto.Method);

            var data = _options.AppId + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];

            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _options.Key1, data));

            var result = await HttpHelper.PostFormAsync(_options.CreateOrderUrl, param);

            // Gửi cả app_trans_id về cho FE lưu lại, sau dùng để query status
            result["app_trans_id"] = app_trans_id;

            // Add Payment And PaymentDetail
            await CreatePaymentAsync(dto, app_trans_id);

            return result;
        }

        public async Task<PaymentDto?> CreatePaymentAsync(PaymentDto dto, string appTransId)
        {
            var payment = _mapper.Map<Payment>(dto);
            payment.AppTransId = appTransId;

            payment.PaymentDetails = new List<PaymentDetail>();
            dto.CourseDtos.ForEach(c =>
            {
                payment.PaymentDetails.Add(new PaymentDetail
                {
                    CourseId = c.CourseId,
                    Price = c.Price,
                });
            });

            await _repo.AddAsync(payment);
            return dto;
        }

        public async Task<IEnumerable<PaymentDto>?> GetRecentInvoices(int userId)
        {
            var query = _repo.GetQueryable();
            var recentInvoices = await query
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();
            var paymentRecent = _mapper.Map<IEnumerable<PaymentDto>>(recentInvoices);

            foreach (var paymentDto in paymentRecent)
            {
                foreach (var payment in recentInvoices.Where(rI => rI.AppTransId == paymentDto.AppTransId))
                {
                    foreach (var paymentDetail in payment.PaymentDetails)
                    {
                        var course = await _courseRepo.GetByIdAsync(paymentDetail.CourseId);
                        paymentDto.CourseDtos.Add(_mapper.Map<CourseDto>(course));
                    }
                }

            }
            return paymentRecent;
        }

        public async Task<Dictionary<string, object>> GetStatusAsync(string appTransId)
        {
            var param = new Dictionary<string, string>();
            param.Add("app_id", _options.AppId);
            param.Add("app_trans_id", appTransId);
            var data = _options.AppId + "|" + appTransId + "|" + _options.Key1;

            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _options.Key1, data));

            var result = await HttpHelper.PostFormAsync(_options.QueryOrderUrl, param);

            return result;
        }

        public async Task<Dictionary<string, object>> HandleCallback(ZaloPayCallbackDto cbdata)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var dataStr = cbdata.data;
                var reqMac = cbdata.mac;

                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _options.Key2, dataStr);

                Console.WriteLine("dataStr: " + cbdata.data);
                Console.WriteLine("reqMac : " + reqMac);
                Console.WriteLine("localMac: " + mac);


                // Kiem tra callback hop le (den tu zalopay server)
                if (!reqMac.Equals(mac))
                {
                    result["return_code"] = -1;
                    result["return_message"] = "Mac không hợp lệ";

                }
                else
                {
                    // thanh toán thành công
                    // merchant cập nhật trạng thái cho đơn hàng
                    var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);

                    using ( var scope = _scopeFactory.CreateScope())
                    {

                        var paymentRepo = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                        var enrollRepo = scope.ServiceProvider.GetRequiredService<IEnrollmentRepository>();

                        var paymentDto = await UpdatePaymentStatusAsync(dataJson["app_trans_id"].ToString(), "Paid", paymentRepo);

                        // Enrollment
                        //var enrollments = new List<Enrollment>();
                        if (paymentDto != null && paymentDto.CourseDtos.Count() > 0)
                        {
                            foreach (var item in paymentDto.CourseDtos)
                            {
                                //enrollments.Add(new Enrollment
                                //{
                                //    UserId = paymentDto.UserId,
                                //    CourseId = item.CourseId,
                                //});
                                var enroll = new Enrollment
                                {
                                    UserId = paymentDto.UserId,
                                    CourseId = item.CourseId,
                                };
                                //await _enrollRepo.AddAsync(enroll);
                                await enrollRepo.AddAsyncNoSave(enroll);
                            }
                            await enrollRepo.SaveChangeAsync();
                        }
                    }

                    Console.WriteLine("Update order status success, app_trans_id = " + dataJson["app_trans_id"]);
                    result["return_code"] = 1;
                    result["return_message"] = "Thanh toán thành công";
                }
            }
            catch (Exception ex)
            {
                result["return_code"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result["return_message"] = ex.Message;
            }

            return result;
        }

        public async Task<PaymentDto?> UpdatePaymentStatusAsync(string appTransId, string status, IPaymentRepository paymentRepo)
        {
            var payment = await paymentRepo.GetByAppTransId(appTransId);
            if (payment == null)
            {
                return null;
            }
            payment.Status = status;
            await paymentRepo.UpdateAsync(payment);

            var paymentDto = _mapper.Map<PaymentDto>(payment);

            foreach (var item in payment.PaymentDetails)
            {
                paymentDto.CourseDtos.Add(_mapper.Map<CourseDto>(item.Course));
            }

            return paymentDto;
        }

    }
}

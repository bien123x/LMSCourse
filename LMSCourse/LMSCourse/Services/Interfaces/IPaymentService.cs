using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.PaymentDto;
using LMSCourse.DTOs.ZaloPay;
using LMSCourse.Repositories.Interfaces;

namespace LMSCourse.Services.Interfaces
{
    public interface IPaymentService 
    {
        Task<Dictionary<string, object>> CreateOrderAsync(PaymentDto dto);
        Task<Dictionary<string, object>> GetStatusAsync(string appTransId);
        Task<Dictionary<string, object>> HandleCallback(ZaloPayCallbackDto cbdata);
        Task<PaymentDto?> CreatePaymentAsync(PaymentDto dto, string appTransId);
        Task<PaymentDto?> UpdatePaymentStatusAsync(string appTransId, string status, IPaymentRepository paymentRepo);
        Task<IEnumerable<PaymentDto>?> GetRecentInvoices(int userId);
    }
}

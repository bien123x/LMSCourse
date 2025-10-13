using LMSCourse.DTOs.PaymentDto;
using LMSCourse.DTOs.ZaloPay;
using LMSCourse.Services;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentDto dto)
        {

            var result = await _paymentService.CreateOrderAsync(dto);
            
            return Ok(result);
        }

        [HttpPost("callback")]
        public IActionResult Callback([FromBody] ZaloPayCallbackDto cbdata)
        {
            var result = _paymentService.HandleCallback(cbdata);

            return Ok(result);
        }

        [HttpPost("query-status")]
        public async Task<IActionResult> QueryStatus([FromBody] string appTransid)
        {
            var result = await _paymentService.GetStatusAsync(appTransid);

            return Ok(result);
        }

        [HttpGet("get-recent-invoices")]
        public async Task<IActionResult> GetRecentInvoices()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var paymentRecent = await _paymentService.GetRecentInvoices(int.Parse(userId));

            return Ok(paymentRecent);
        }
    }
}

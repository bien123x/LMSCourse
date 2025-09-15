using LMSCourse.Models;
using LMSCourse.Repositories;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _service;

        public SettingsController(ISettingsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPolicy()
        {
            var policy = await _service.GetPasswordPolicy();
            if (policy == null) return NotFound("Không có policy");
            return Ok(policy);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePolicy([FromBody] PasswordPolicy policy)
        {
            var policyUpdate = await _service.UpdatePasswordPolicy(policy);

            if (policyUpdate == null) return NotFound(policy);
            return Ok(new { Message = "Lưu chính sách mật khẩu thành công." });
        }

        [HttpPost]
        public async Task<IActionResult> ValidatePassword([FromBody]string password)
        {
            var (isValid, errors) = await _service.ValidateAsync(password);

            return Ok(new { isValid, errors });
        }
    }
}

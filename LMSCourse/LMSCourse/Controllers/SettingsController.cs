using LMSCourse.DTOs.Setting;
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
        private readonly ISettingService _service;

        public SettingsController(ISettingService service)
        {
            _service = service;
        }

        [HttpGet("identity")]
        public async Task<IActionResult> GetIdentitySettings()
        {
            var identitySetting = await _service.GetIdentitySettingAsync();
            return Ok(identitySetting);
        }

        [HttpPut("identity")]
        public async Task<IActionResult> UpdateIdentitySettings(IdentitySettingDto identitySettingDto)
        {
            var identitySetting = await _service.UpdateIdentitySettingAsync(identitySettingDto);
            return Ok(identitySetting);
        }

        [HttpPost]
        public async Task<IActionResult> ValidatePassword([FromBody]string password)
        {
            var (isValid, errors) = await _service.ValidateAsync(password);

            return Ok(new { isValid, errors });
        }
    }
}

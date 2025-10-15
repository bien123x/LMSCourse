using LMSCourse.DTOs.Setting;
using LMSCourse.Models;
using LMSCourse.Repositories;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;

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
        [Authorize(Policy = PERMISSION.System.Settings.Module)]
        public async Task<IActionResult> GetIdentitySettings()
        {
            var identitySetting = await _service.GetIdentitySettingAsync();
            return Ok(identitySetting);
        }

        [HttpPut("identity")]
        [Authorize(Policy = PERMISSION.System.Settings.Edit)]
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

        [HttpGet("user-policy")]
        public async Task<IActionResult> GetUserSettingAsync()
        {
            var userPolicy = await _service.GetUserSettingAsync();

            return Ok(userPolicy);
        }

    }
}

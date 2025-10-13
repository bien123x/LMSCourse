using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionsController : ControllerBase
    {
        private IPermissionService _service;
        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetTreePermissionsAsync() { 
            var permissions = await _service.GetTreePermissionsAsync();
            return Ok(permissions);
        }
    }
}

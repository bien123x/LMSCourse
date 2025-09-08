using LMSCourse.DTOs.Role;
using LMSCourse.DTOs.User;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            this._roleService = roleService;
        }

        [HttpGet("view-roles-dto")]
        [Authorize(Policy = PERMISSION.ViewRoles)]
        public async Task<IActionResult> GetViewRoles()
        {
            var viewRolesDto = await _roleService.GetViewRoles();

            if (viewRolesDto == null)
            {
                return NotFound();
            }

            return Ok(viewRolesDto);
        }

        [HttpPost("create-role")]
        [Authorize(Policy = PERMISSION.CreateRoles)]
        public async Task<IActionResult> CreateRole(RoleDto dto)
        {
            var viewRoleDto = await _roleService.CreateRole(dto);
            if (viewRoleDto == null)
                return BadRequest("Quyền đã tồn tại!");

            return Ok(viewRoleDto);
        }

    }
}

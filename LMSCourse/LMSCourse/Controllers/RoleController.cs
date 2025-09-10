using LMSCourse.DTOs.Role;
using LMSCourse.DTOs.User;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;
using System.Threading.Tasks;

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

        [HttpPut("edit-role/{roleId:int}")]
        [Authorize(Policy = PERMISSION.EditRoles)]
        public async Task<IActionResult> EditRole(int roleId, RoleDto dto)
        {
            var viewRoleDto = await _roleService.UpdateRole(roleId, dto);

            if (viewRoleDto == null)
                return BadRequest("Đã tồn tại quyền này");
            return Ok(viewRoleDto);
        }

        [HttpGet("get-permissions/{roleId:int}")]
        [Authorize(Policy = PERMISSION.ViewRoles)]
        public async Task<IActionResult> GetPermissionsById(int roleId)
        {
            var permissions = await _roleService.GetPermissionsById(roleId);

            if (permissions.Count == 0) return Ok(new List<string>());

            return Ok(permissions);
        }

        [HttpPut("update-permissions/{roleId:int}")]
        [Authorize(Policy = PERMISSION.EditRoles)]
        public async Task<IActionResult> EditRolePermissions(int roleId, List<string> permissionsName)
        {
            var permissions = await _roleService.UpdatePermissions(roleId, permissionsName);

            if (permissions == null) return NotFound("Quyền không tồn tại!");

            return Ok(permissions);
        }

        [HttpGet("count-users-role/{roleId:int}")]
        [Authorize(Policy = PERMISSION.ViewRoles)]
        public async Task<IActionResult> CountUserRoles(int roleId)
        {
            var countUser = await _roleService.CountUserByRoleId(roleId);

            if (countUser > 0)
                return Ok(new {msg = $"{countUser} người dùng có quyền này." } );
            else if (countUser == -1)
                return BadRequest("Không có quyền này");
            return Ok();
        }

        [HttpDelete("delete-role-unassign/{roleId:int}")]
        [Authorize(Policy = PERMISSION.DeleteRoles)]
        public async Task<IActionResult> DeleteRoleUnassign(int roleId)
        {
            var isDelete = await _roleService.DeleteRole(roleId);
            if (isDelete)
                return Ok();
            return NotFound();
        }

        [HttpDelete("delete-role-assign/{roleId:int}/{roleIdAssign:int}")]
        [Authorize(Policy = PERMISSION.DeleteRoles)]
        public async Task<IActionResult> DeleteRoleAssign(int roleId, int roleIdAssign)
        {
            var isAssign = await _roleService.AssignRoleUserDelete(roleId, roleIdAssign);

            var isDelete = await _roleService.DeleteRole(roleId);
            if (isDelete && isAssign)
                return Ok();
            return NotFound();
        }

        [HttpGet("roles-minus-role/{roleId:int}")]
        [Authorize(Policy = PERMISSION.ViewRoles)]
        public async Task<IActionResult> GetRolesMinusRoleId(int roleId)
        {
            var roles = await _roleService.GetRolesMinusRoleId(roleId);
            return Ok(roles);
        }

    }
}

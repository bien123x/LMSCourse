using LMSCourse.DTOs.Page;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISettingsService _settingsService;
        public UserController(IUserService userService, ISettingsService settingsService)
        {
            _userService = userService;
            _settingsService = settingsService;
        }

        [HttpGet("view-user/{userId:int}")]
        [Authorize(Policy = PERMISSION.ViewUsers)]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var viewUserDto = await _userService.GetUserByIdAsync(userId);

            if (viewUserDto == null)
                return NotFound();

            return Ok(viewUserDto);
        }

        [HttpGet("all-view-user")]
        [Authorize(Policy = PERMISSION.ViewUsers)]
        public async Task<IActionResult> GetAllViewUsersDto()
        {
            var usersDto = await _userService.GetAllViewUser();

            return Ok(usersDto.Select(u => u));
        }

        [HttpPost("add-user")]
        [Authorize(Policy = PERMISSION.CreateUsers)]
        public async Task<IActionResult> AddUserDto(UserDto userDto)
        {
            var (isValid, errors) = await _settingsService.ValidateAsync(userDto.PasswordHash);

            if (!isValid)
                return BadRequest(new { Errors = errors });
            var userAdd = await _userService.AddUserAsync(userDto);
            if (userAdd == null) return BadRequest("Tên đang nhập/Email đã tồn tại!");
            return Ok(userAdd);
        }

        [HttpPut("edit-user/{userId:int}")]
        [Authorize(Policy = PERMISSION.EditUsers)]
        public async Task<IActionResult> EditUserDto(int userId, EditUserDto editUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEdit = await _userService.EditUserDto(userId, editUserDto);


            if (userEdit == null)
                return BadRequest(
                    new { Message = "Không thể cập nhật user. Có thể user không tồn tại hoặc Username/Email đã được dùng." }
                );
            return Ok(userEdit);
        }

        [HttpGet("roles-name")]
        [Authorize(Policy = PERMISSION.ViewRoles)]
        public async Task<IActionResult> GetRolesName()
        {
            var rolesName = await _userService.GetRolesName();
            return Ok(rolesName);
        }

        [HttpGet("permissions-name/{userId:int}")]
        [Authorize(Policy = PERMISSION.ViewUsers)]
        public async Task<IActionResult> GetPermissions(int userId)
        {
            var userPermissions = await _userService.GetUserPermissionsNameById(userId);
            var rolePermissions = await _userService.GetPermissionsNameByIdAsync(userId);
            return Ok(new UserPermissionsDto { UserPermissions = userPermissions, RolePermissions = rolePermissions });
        }
        [HttpPut("user-permissions/{userId:int}")]
        public async Task<IActionResult> UpdateUserPermissions(int userId, List<string> permissions)
        {
            var updatePermissions = await _userService.UpdateUserPermissions(userId, permissions);
            if (updatePermissions == null) return BadRequest("Không có người dùng này");

            return Ok(updatePermissions);
        }

        [HttpPut("reset-password/{userId:int}")]
        [Authorize(Policy = PERMISSION.EditUsers)]
        public async Task<IActionResult> ResetPassword(int userId, SetPassword resetDto)
        {
            await _userService.ResetPassword(userId, resetDto.PasswordHash);
            return Ok();
        }
        [HttpDelete("delete-user/{userId:int}")]
        [Authorize(Policy = PERMISSION.DeleteUsers)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var isDelete = await _userService.DeleteUser(userId);
            if (isDelete)
                return Ok();
            return BadRequest("Không có user này");
        }

        [HttpPost("users")]
        [Authorize(Policy = PERMISSION.ViewUsers)]
        public async Task<ActionResult<PagedResult<ViewUserDto>>> GetPagedUsers([FromBody] QueryDto query)
        {
            var result = await _userService.GetPagedUsers(query);

            return Ok(result);
        }
    }
}

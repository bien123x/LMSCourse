using LMSCourse.DTOs.Page;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;
using System.Security.Claims;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISettingService _settingsService;
        public UserController(IUserService userService, ISettingService settingsService)
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

            var addUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(addUserId))
                return NotFound("Người thêm không tồn tại!");
            var userAdd = await _userService.AddUserAsync(userDto, int.Parse(addUserId));
            if (userAdd == null) return BadRequest("Tên đang nhập/Email đã tồn tại hoặc người tạo không xác thực!");
            return Ok(userAdd);
        }

        [HttpPut("edit-user/{userId:int}")]
        [Authorize(Policy = PERMISSION.EditUsers)]
        public async Task<IActionResult> EditUserDto(int userId, EditUserDto editUserDto)
        {
            var editUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(editUserId))
                return NotFound(new {
                    Message = "Người sửa không tồn tại!"});
            
            var userEdit = await _userService.EditUserDto(userId, editUserDto, int.Parse(editUserId));

            if (userEdit == null)
                return BadRequest(new {Message = "Sửa người dùng thất bại! Đã tồn tại tên đăng nhập/Email!"});
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

        [HttpPut("change-password/{userId:int}")]
        public async Task<IActionResult> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var result = await _userService.ChangePasswordByIdAsync(userId, dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("lock-user/{userId:int}")]
        public async Task<IActionResult> LockUserAsync(int userId, LockEndTimeDto dto)
        {
            var result = await _userService.LockUserByIdAsync(userId, dto.LockEndTime);

            if (result.Success)
            {
                return Ok(result);
            }
            else
                return NotFound(result.Message);
        }

        [HttpPut("unlock-user/{userId:int}")]
        public async Task<IActionResult> UnlockUserAsync(int userId)
        {
            var result = await _userService.UnLockUserByIdAsync(userId);

            if (result.Success) 
                return Ok(result);
            else
                return NotFound(result.Message);
        }
    }
}

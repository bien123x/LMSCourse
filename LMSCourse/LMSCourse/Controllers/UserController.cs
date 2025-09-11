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
        public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}

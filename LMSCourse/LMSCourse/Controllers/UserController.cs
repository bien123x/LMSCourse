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

        [HttpGet("{id}")]
        [Authorize(Policy = PERMISSION.ViewUsers)]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);

            if (userDto == null)
                return NotFound();

            return Ok(userDto);
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
            if (userAdd == null) return BadRequest();
            return Ok(userAdd);
        }
    }
}

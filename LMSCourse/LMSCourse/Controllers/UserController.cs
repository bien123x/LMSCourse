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
    }
}

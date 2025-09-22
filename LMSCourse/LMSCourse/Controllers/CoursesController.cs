using LMSCourse.DTOs.Course;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) {
            _courseService = courseService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var coursesDto = await _courseService.GetAllAsync();
            return Ok(coursesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync(CourseCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var course = await _courseService.CreateAsync(dto);
            return Ok("");
        }
    }
}

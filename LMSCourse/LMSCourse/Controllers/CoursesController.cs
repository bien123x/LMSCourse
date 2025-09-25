using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Page_Sort_Filter;
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
            var course = await _courseService.CreateAsync(dto);
            return Ok("");
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetCourseFiltersAsync()
        {
            var courseFilters = await _courseService.GetCourseFilterAsync();
            return Ok(courseFilters);
        }

        [HttpPost("all-with-filter")]
        public async Task<IActionResult> GetAllWithFilter([FromBody] QueryCourseDto dto)
        {
            var pageCourses = await _courseService.GetAllWithFilter(dto);
            return Ok(pageCourses);
        }
        [HttpGet("{courseId:int}")]
        public async Task<IActionResult> GetCourseByIdAsync(int courseId)
        {
            var result = await _courseService.GetCourseByIdAsync(courseId);

            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        
    }
}

using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;
using System.Security.Claims;

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

        private int? UserId =>
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : (int?)null;


        [HttpGet("all/{userId:int}")]
        [Authorize(Policy = PERMISSION.Students.Courses.Module)]
        public async Task<IActionResult> GetAll(int userId)
        {
            var coursesDto = await _courseService.GetAllAsync(userId);
            return Ok(coursesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync(CourseCreateUpdateDto dto)
        {
            var course = await _courseService.CreateAsync(dto);
            return Ok(course);
        }

        [HttpGet("filters")]
        [Authorize(Policy = PERMISSION.Students.Courses.Module)]
        public async Task<IActionResult> GetCourseFiltersAsync()
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var courseFilters = await _courseService.GetCourseFilterAsync(UserId.Value);
            return Ok(courseFilters);
        }

        [HttpPost("all-with-filter")]
        [Authorize(Policy = PERMISSION.Students.Courses.Module)]
        public async Task<IActionResult> GetAllWithFilter([FromBody] QueryCourseDto dto)
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var pageCourses = await _courseService.GetAllWithFilter(dto, UserId.Value);
            return Ok(pageCourses);
        }
        [HttpGet("{courseId:int}")]
        [Authorize(Policy = PERMISSION.Students.Courses.ViewDetails.Module)]
        public async Task<IActionResult> GetCourseByIdAsync(int courseId)
        {
            var result = await _courseService.GetCourseByIdAsync(courseId);

            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost("enrolled")]
        [Authorize(Policy = PERMISSION.Students.Enrollments.Module)]
        public async Task<IActionResult> GetAllCoursesEnrolledAsync([FromBody] QueryCourseEnrolledDto dto)
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var coursesEnrolled = await _courseService.GetCoursesEnrolled(UserId.Value, dto);

            return Ok(coursesEnrolled);
        }

        [HttpPost("get-courses-by-listId")]
        [Authorize(Policy = PERMISSION.Students.Courses.Module)]
        public async Task<IActionResult> GetCoursesByListId(List<int> courseIds)
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var coursesDto = await _courseService.GetCoursesByListId(courseIds, UserId.Value);

            return Ok(coursesDto);
        }

        [HttpGet("remaining-apacity/{courseId}")]
        public async Task<IActionResult> GetRemainingCapacity(int courseId)
        {
            var remainingCapacity = await _courseService.GetRemainingCapacity(courseId);

            return Ok(remainingCapacity);
        }
    }
}

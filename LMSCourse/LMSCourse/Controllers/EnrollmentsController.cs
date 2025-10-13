using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _service;

        private int? UserId =>
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : (int?)null;
        public EnrollmentsController(IEnrollmentService service)
        {
            _service = service;
        }

        [HttpGet("{courseId:int}")]
        public async Task<IActionResult> GetEnrollmentByIdAsync(int courseId)
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var enrollment = await _service.GetEnrollmentByIdAsync(courseId, UserId.Value);
            return Ok(enrollment);
        }

        [HttpGet("dashboard-enrollment-course")]
        public async Task<IActionResult> GetDashboardEnrollmentCourseByIdAsync()
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var dashboardEnrollment = await _service.GetDashboardEnrollmentCourseByIdAsync(UserId.Value);

            return Ok(dashboardEnrollment);
        }

        [HttpGet("update-status/{courseId:int}")]
        public async Task<IActionResult> UpdateStatus(int courseId)
        {
            if (UserId == null)
            {
                return Unauthorized("Chưa đăng nhập");
            }
            var enrollment = await _service.UpdateStatus(courseId, UserId.Value);
            return Ok(enrollment);
        }

        [HttpGet("is-exist-certificate/{enrollmentId:int}")]
        public async Task<IActionResult> IsExistCertificate(int enrollmentId)
        {
            var isExist = await _service.IsExistCertificate(enrollmentId);
            return Ok(isExist);
        }
    }
}

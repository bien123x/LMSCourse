using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseConstants;
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
        [Authorize(Policy = PERMISSION.Students.Enrollments.ViewDetails.Module)]
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
        [Authorize(Policy = PERMISSION.Students.Enrollments.Module)]
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

        [HttpGet("update-progress/{enrollmentId:int}")]
        public async Task<IActionResult> UpdateProgress(int enrollmentId)
        {
            var enrollment = await _service.UpdateProgress(enrollmentId);
            if (enrollment == null) return NotFound("Không tồn tại khoá học này!");
            return Ok(enrollment);
        }
    }
}

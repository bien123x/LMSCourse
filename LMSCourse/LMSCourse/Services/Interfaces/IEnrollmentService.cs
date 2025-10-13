using LMSCourse.DTOs.Enrollment;

namespace LMSCourse.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDto?> GetEnrollmentByIdAsync(int courseId, int userId);
        Task<DashboardEnrollmentCourseDto?> GetDashboardEnrollmentCourseByIdAsync(int userId);
        Task<EnrollmentDto> UpdateStatus(int courseId, int userId);
        Task<bool> IsCompleteCourse(int courseId, int userId);
        Task<bool> IsExistCertificate(int enrollmentId);
    }
}

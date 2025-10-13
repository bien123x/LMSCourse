using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<Enrollment?> GetWithCourseByIdAsync(int courseId, int userId);
        IQueryable<Enrollment> GetAllQueryable(int userId);
        Task<Enrollment?> GetWithCertificateByIdAsync(int enrollmentId);
        Task AddAsyncNoSave(Enrollment enrollment);
        Task SaveChangeAsync();
    }
}

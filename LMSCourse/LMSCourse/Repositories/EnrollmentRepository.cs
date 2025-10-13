using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace LMSCourse.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Enrollment?> GetWithCourseByIdAsync(int courseId, int userId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.CourseTopics)
                        .ThenInclude(t => t.Lessons)
                .Include(e => e.Course)
                    .ThenInclude(c => c.FaqGroups)
                        .ThenInclude(f => f.FaqItems)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public IQueryable<Enrollment> GetAllQueryable(int userId)
        {
            return _context.Enrollments.Where(e => e.UserId == userId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.CourseTopics)
                        .ThenInclude(t => t.Lessons)
                .Include(e => e.Course)
                    .ThenInclude(c => c.FaqGroups)
                        .ThenInclude(f => f.FaqItems)
                .AsQueryable();
        }

        public async Task<Enrollment?> GetWithCertificateByIdAsync(int enrollmentId)
        {
            return await _context.Enrollments
               .Include(e => e.Certificate)
               .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public async Task AddAsyncNoSave(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

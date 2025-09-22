using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context) { 
        }

        public async Task<IEnumerable<Course>> GetAllWithDataDto()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .Include(c => c.Level)
                .Include(c => c.Language)
                .Include(c => c.FaqGroups)
                    .ThenInclude(fg => fg.FaqItems)
                .Include(c => c.CourseTopics)
                .ToListAsync();
        }
    }
}

using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Quiz>?> GetQuizzesByCourseId(int courseId)
        {
            var quiz = await _context.Quizzes.Where(q => q.CourseId == courseId).ToListAsync();
            return quiz;
        }

        public async Task<int> GetTotalMarksByCourseId(int courseId)
        {
            return await _context.Quizzes.Where(q => q.CourseId == courseId).Select(q => q.TotalMarks).SumAsync();
        }
    }
}

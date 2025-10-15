using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class UserQuizRepository : GenericRepository<UserQuiz>, IUserQuizRepository
    {
        public UserQuizRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<UserQuiz> GetAllQuery()
        {
            return _context.UserQuizzes.AsQueryable();
        }

        public async Task<int> GetMarkByCourseId(int courseId, int userId)
        {
            return await _context.UserQuizzes
                .Include(uq => uq.Quiz)
                .Where(uq => uq.UserId == userId && uq.Quiz.CourseId == courseId)
                .GroupBy(uq => uq.QuizId) // nhóm theo từng quiz
                .Select(g => g.Max(uq => uq.Score))
                .SumAsync();
        }

        public async Task<UserQuiz?> GetUserQuizHasMaxScore(int quizId, int userId)
        {
            return await _context.UserQuizzes.Where(uq => uq.QuizId == quizId && uq.UserId == userId).OrderByDescending(uq => uq.Score)
        .FirstOrDefaultAsync();
        }

        public async Task<UserQuiz?> GetWithQuizById(int userQuizId)
        {
            return await _context.UserQuizzes.Include(uq => uq.Quiz)
                .FirstOrDefaultAsync(uq => uq.UserQuizId == userQuizId);
        }
    }
}

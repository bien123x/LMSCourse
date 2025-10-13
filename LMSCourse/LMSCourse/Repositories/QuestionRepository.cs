using LMSCourse.Data;
using LMSCourse.DTOs.Question;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Question>> GetQuestionsWithAnswerByQuizId(int quizId)
        {
            return await _context.Questions.Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Question?> GetWithAnswer(int questionId)
        {
            return await _context.Questions.Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);
        }
    }
}

using LMSCourse.DTOs.Question;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsWithAnswerByQuizId(int quizId);
        Task<Question?> GetWithAnswer(int questionId);
    }
}

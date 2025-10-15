using LMSCourse.DTOs.Quiz;

namespace LMSCourse.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizViewDto>?> GetQuizzesByCourseId(int courseId);
        Task<QuizViewDto?> CreateQuiz(QuizDto dto);
        Task<QuizViewDto?> GetQuizById(int quizId);
        Task<int> CountUserQuizPassQuiz(int courseId, int userId);
    }
}

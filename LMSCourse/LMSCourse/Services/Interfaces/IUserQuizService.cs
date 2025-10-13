using LMSCourse.DTOs.UserQuiz;

namespace LMSCourse.Services.Interfaces
{
    public interface IUserQuizService
    {
        Task<UserQuizViewDto> CreateUserQuizWithUserAnswers(UserQuizDto dto);
        Task<bool> IsCorrectAnswer(int questionId, int? answerId);
        Task<bool> IsPassQuiz(int userQuizId);
        Task<IEnumerable<LatestQuizDto>> GetLastestQuizzes(int userId);
    }
}

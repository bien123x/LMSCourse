using LMSCourse.DTOs.Question;

namespace LMSCourse.Services.Interfaces
{
    public interface IQuestionSevice
    {
        Task<QuestionViewDto> CreateQuestionWithAnswerAsync(QuestionDto dto);
        Task<IEnumerable<QuestionViewDto>> GetQuestionsWithAnswerByQuizId(int quizId);
    }
}

using LMSCourse.Models;

namespace LMSCourse.DTOs.UserQuiz
{
    public class UserQuizViewDto
    {
        public int UserQuizId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<UserAnswerViewDto> UserAnswers { get; set; } = new List<UserAnswerViewDto>();
    }

    public class UserAnswerViewDto
    {
        public int UserAnswerId { get; set; }
        public int UserQuizId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
    }
}

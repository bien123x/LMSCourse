using LMSCourse.Models;

namespace LMSCourse.DTOs.UserQuiz
{
    public class UserQuizDto
    {
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<UserAnswerDto> UserAnswers { get; set; } = new List<UserAnswerDto>();
    }

    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
    }
}

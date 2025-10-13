using LMSCourse.Models;

namespace LMSCourse.DTOs.Question
{
    public class QuestionDto
    {
        public string QuestionStr { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class AnswerDto
    {
        public string AnswerStr { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}

namespace LMSCourse.DTOs.Question
{
    public class QuestionViewDto
    {
        public int QuestionId { get; set; }
        public string QuestionStr { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public List<AnswerViewDto> Answers { get; set; } = new List<AnswerViewDto>();
    }

    public class AnswerViewDto
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerStr { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}

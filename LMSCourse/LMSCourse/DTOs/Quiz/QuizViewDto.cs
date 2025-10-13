using LMSCourse.Models;

namespace LMSCourse.DTOs.Quiz
{
    public class QuizViewDto
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int NoOfQuestions { get; set; } = 0;
        public int TotalMarks { get; set; } = 100;
        public int PassMark { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

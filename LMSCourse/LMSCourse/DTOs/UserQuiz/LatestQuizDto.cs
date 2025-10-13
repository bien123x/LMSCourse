namespace LMSCourse.DTOs.UserQuiz
{
    public class LatestQuizDto
    {
        public int UserQuizId { get; set; }
        public int CountCorrectAnswer { get; set; }
        public int NoOfQuestions { get; set; }
        public string TitleQuiz { get; set; } = string.Empty;
        public int PercentageScore { get; set; }
    }
}

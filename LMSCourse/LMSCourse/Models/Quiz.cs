namespace LMSCourse.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int NoOfQuestions { get; set; } = 0;
        public int TotalMarks { get; set; } = 100;
        public int PassMark { get; set; }
        public TimeSpan Duration { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public Course? Course { get; set; }
        public ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionStr { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public ICollection<UserAnswer>? UserAnswers { get; set; } = new List<UserAnswer>();
    }

    public class Answer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerStr { get; set; } = string.Empty ;
        public bool IsCorrect { get; set; } = false;
        public Question? Question { get; set; }
    }

    public class UserQuiz
    {
        public int UserQuizId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Quiz? Quiz { get; set; }
        public User? User { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }

    public class UserAnswer
    {
        public int UserAnswerId { get; set; } 
        public int UserQuizId { get; set; } 
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; } 

        public UserQuiz? UserQuiz { get; set; } 
        public Question? Question { get; set; } 
        public Answer? Answer { get; set; }   
    }
}

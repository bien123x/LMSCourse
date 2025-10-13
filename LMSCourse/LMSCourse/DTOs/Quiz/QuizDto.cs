using FluentValidation;

namespace LMSCourse.DTOs.Quiz
{
    public class QuizDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int NoOfQuestions { get; set; } = 0;
        public int TotalMarks { get; set; } = 100;
        public int PassMark { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class QuizDtoValidation : AbstractValidator<QuizDto>
    {
        public QuizDtoValidation() {
            RuleFor(q => q.CourseId).NotNull().WithMessage("Chưa chọn khoá học!");
            RuleFor(q => q.Title).NotEmpty().WithMessage("Tiêu đề không được để trống!");
        }
    }
}

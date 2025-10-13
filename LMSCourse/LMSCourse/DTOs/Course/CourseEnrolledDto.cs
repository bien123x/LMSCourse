namespace LMSCourse.DTOs.Course
{
    // Dto chi tiết (cho đã đăng ký)
    public class CourseEnrolledDto : CourseDto
    {
        // Ghi đè CourseTopics: chứa Lesson đầy đủ
        public new List<CourseTopicDetailDto> CourseTopics { get; set; } = new();
    }

    public class CourseTopicDetailDto
    {
        public int CourseTopicId { get; set; }
        public string Title { get; set; } = string.Empty;

        public List<LessonDetailDto> Lessons { get; set; } = new();
    }

    public class LessonDetailDto
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;

        public string? LessonContent { get; set; }
        public string? Description { get; set; }
        public bool IsFreeOrPremium { get; set; }

        public int CourseTopicId { get; set; }
    }
}

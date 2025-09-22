namespace LMSCourse.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? LessonContent { get; set; } // text, video link, file
        public string? Description { get; set; }
        public bool IsFreeOrPremium { get; set; } = true;

        // Liên kết Topic
        public int CourseTopicId { get; set; }
        public CourseTopic? CourseTopic { get; set; }
    }
}

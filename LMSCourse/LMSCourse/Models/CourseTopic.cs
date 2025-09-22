namespace LMSCourse.Models
{
    public class CourseTopic
    {
        public int CourseTopicId { get; set; }
        public string Title { get; set; } = string.Empty;

        // Liên kết Course
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        // Mỗi Topic có nhiều Lesson
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}

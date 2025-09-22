namespace LMSCourse.DTOs.Course
{
    public class FaqItemDto
    {
        public int FaqItemId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}

namespace LMSCourse.DTOs.Course
{
    public class FaqGroupDto
    {
        public int FaqGroupId { get; set; }
        public string Title { get; set; } = string.Empty;

        // Các FAQ items
        public List<FaqItemDto> FaqItems { get; set; } = new List<FaqItemDto>();
    }
}

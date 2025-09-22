namespace LMSCourse.Models
{
    public class FaqGroup
    {
        public int FaqGroupId { get; set; }
        public string Title { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public ICollection<FaqItem> FaqItems { get; set; } = new List<FaqItem>();
    }
}

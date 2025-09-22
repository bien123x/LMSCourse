namespace LMSCourse.Models
{
    public class FaqItem
    {
        public int FaqItemId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public int FaqGroupId { get; set; }
        public FaqGroup? FaqGroup { get; set; }
    }
}

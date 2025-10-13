namespace LMSCourse.Models
{
    public class CertificateTemplate
    {
        public int CertificateTemplateId { get; set; }
        public int TeacherId { get; set; }
        public User? Teacher { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string TemplateUrl { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; } = false;
    }
}

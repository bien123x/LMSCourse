using LMSCourse.Models;

namespace LMSCourse.DTOs.Certificate
{
    public class CertificateViewDto
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public string CertificateUrl { get; set; } = string.Empty;
        public int Marks { get; set; }
        public int OutOf { get; set; }
    }
}

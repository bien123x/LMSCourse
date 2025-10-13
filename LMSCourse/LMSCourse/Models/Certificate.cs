namespace LMSCourse.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public string CertificateUrl { get; set; } = string.Empty;
        // Trạng thái (Issued, Revoked, v.v.)
        public string Status { get; set; } = "Issued";
        public int CertificateTemplateId { get; set; }
        public CertificateTemplate? CertificateTemplate { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; }
    }
}

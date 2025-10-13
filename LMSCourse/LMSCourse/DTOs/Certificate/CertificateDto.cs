using LMSCourse.Models;

namespace LMSCourse.DTOs.Certificate
{
    public class CertificateDto
    {
        public int courseId { get; set; }
        // Trạng thái (Issued, Revoked, v.v.)
        //public string Status { get; set; } = "Issued";

        public int userId { get; set; }
        public int teacherId { get; set; }
    }
}

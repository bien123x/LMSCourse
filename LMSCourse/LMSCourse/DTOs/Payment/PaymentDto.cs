using LMSCourse.DTOs.Course;
using LMSCourse.Models;

namespace LMSCourse.DTOs.PaymentDto
{
    public class PaymentDto
    {
        public int UserId { get; set; }
        public long Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        //'Pending', 'Paid', 'Failed'
        public string Status { get; set; } = "Pending";
        public string AppTransId { get; set; } = string.Empty;
        public List<CourseDto> CourseDtos { get; set; } = new List<CourseDto>();
    }
}

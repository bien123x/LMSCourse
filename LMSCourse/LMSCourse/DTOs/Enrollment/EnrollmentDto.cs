using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.User;

namespace LMSCourse.DTOs.Enrollment
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public CourseEnrolledDto? Course { get; set; }
        //'Active', 'Cancelled', 'Completed'
        public string Status { get; set; } = "Active";
        public DateTime EnrollDate { get; set; } = DateTime.UtcNow;
        public double Progess { get; set; } = 0;
    }
}

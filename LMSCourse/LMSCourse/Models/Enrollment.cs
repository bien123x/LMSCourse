namespace LMSCourse.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        //'Active', 'Cancelled', 'Completed'
        public string Status { get; set; } = "Active";
        public DateTime EnrollDate { get; set; } = DateTime.UtcNow;
        public double Progess { get; set; } = 0;

        // Liên kết 1-1 đến chứng chỉ
        public Certificate? Certificate { get; set; }
    }
}

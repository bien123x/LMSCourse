using LMSCourse.DTOs.Course;

namespace LMSCourse.DTOs.Enrollment
{
    public class DashboardEnrollmentCourseDto
    {
        public int CountEnrolledCourse { get; set; }
        public int CountActiveCourse { get; set; }
        public int CountCompleteCourse { get; set; }
        public List<EnrollmentDto> RecentEnrolledCourses { get; set; } = new List<EnrollmentDto>();
    }
}

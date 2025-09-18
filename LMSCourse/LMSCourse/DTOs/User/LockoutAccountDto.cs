namespace LMSCourse.DTOs.User
{
    public class LockoutAccountDto
    {
        public int LockoutDuration { get; set; }
        public int FailedAccessCount { get; set; }

    }
}

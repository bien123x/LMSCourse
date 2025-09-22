namespace LMSCourse.DTOs.User
{
    public class ViewUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string Roles { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModificationTime { get; set; } = DateTime.UtcNow;
        public DateTime PasswordUpdateTime { get; set; } = DateTime.UtcNow;
        public DateTime? LockoutEndTime { get; set; }
        public int FailedAccessCount { get; set; } = 0;
    }
}

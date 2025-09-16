namespace LMSCourse.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Email verification
        public bool IsEmailConfirmed { get; set; } = false;
        public string EmailVerificationToken { get; set; } = string.Empty;
        public DateTime? EmailVerificationTokenExpires { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModificationTime { get; set; } = DateTime.Now;
        public DateTime PasswordUpdateTime { get; set; } = DateTime.Now;
        public DateTime? LockoutEndTime { get; set; }
        public int FailedAccessCount { get; set; } = 0;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}

using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Models
{
    public class IdentitySetting
    {
        public int Id { get; set; }
        public PasswordSetting Password { get; set; }
        public LockoutSetting Lockout { get; set; }
        public SignInSetting SignIn { get; set; }
        public UserSetting User { get; set; }
    }

    [Owned]
    public class PasswordSetting
    {
        public int RequiredLength { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool ForceUsersToPeriodicallyChangePassword { get; set; }
        public int PasswordChangePeriodDays { get; set; }
    }

    [Owned]
    public class LockoutSetting
    {
        public bool AllowedForNewUsers { get; set; }
        public int LockoutDuration { get; set; } // seconds
        public int MaxFailedAccessAttempts { get; set; }
    }

    [Owned]
    public class SignInSetting
    {
        public bool RequireConfirmedEmail { get; set; }
        public bool RequireEmailVerificationToRegister { get; set; }
        public bool EnablePhoneNumberConfirmation { get; set; }
        public bool RequireConfirmedPhoneNumber { get; set; }
    }

    [Owned]
    public class UserSetting
    {
        public bool IsUserNameUpdateEnabled { get; set; }
        public bool IsEmailUpdateEnabled { get; set; }
    }
}

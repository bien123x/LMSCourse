using LMSCourse.Models;

namespace LMSCourse.DTOs.Setting
{
    public class IdentitySettingDto
    {
        public PasswordSettingDto Password { get; set; }
        public LockoutSettingDto Lockout { get; set; }
        public SignInSettingDto SignIn { get; set; }
        public UserSettingDto User { get; set; }
    }

    public class PasswordSettingDto
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

    public class LockoutSettingDto
    {
        public bool AllowedForNewUsers { get; set; }
        public int LockoutDuration { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
    }

    public class SignInSettingDto
    {
        public bool RequireConfirmedEmail { get; set; }
        public bool RequireEmailVerificationToRegister { get; set; }
        public bool EnablePhoneNumberConfirmation { get; set; }
        public bool RequireConfirmedPhoneNumber { get; set; }
    }

    public class UserSettingDto
    {
        public bool IsUserNameUpdateEnabled { get; set; }
        public bool IsEmailUpdateEnabled { get; set; }
    }
}

namespace LMSCourse.DTOs.User
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string? EmailVerificationToken { get; set; } = string.Empty;

    }
}

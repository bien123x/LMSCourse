namespace LMSCourse.DTOs.User
{
    public class ResetPasswordDto
    {
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}

namespace LMSCourse.DTOs.User
{
    public class ChangePasswordDto
    {
        public string NowPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

using FluentValidation;

namespace LMSCourse.DTOs.User
{
    public class ChangePasswordDto
    {
        public string NowPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            // Mật khẩu hiện tại không được để trống
            RuleFor(x => x.NowPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu hiện tại.");

            // Mật khẩu mới không được để trống
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.");

            // Xác nhận mật khẩu mới
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập lại mật khẩu mới.")
                .Equal(x => x.NewPassword).WithMessage("Mật khẩu xác nhận không khớp.");
        }
    }
}

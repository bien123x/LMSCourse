using FluentValidation;

namespace LMSCourse.DTOs.User
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string? EmailVerificationToken { get; set; } = string.Empty;

    }

    public class RegisterValidation : AbstractValidator<RegisterDto>
    {
        public RegisterValidation() {
            RuleFor(register => register.UserName)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
                .MinimumLength(3).WithMessage("Tên đăng nhập phải có ít nhất 3 ký tự")
                .MaximumLength(20).WithMessage("Tên đăng nhập tối đa 20 ký tự")
                .Matches("^[a-zA-Z0-9_.-]*$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang");
            // Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");
        }

    }
}

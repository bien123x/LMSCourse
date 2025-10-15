using FluentValidation;

namespace LMSCourse.DTOs.User
{
    public class EditUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<string> Roles { get; set; } = new List<string>();
    }

    public class EditUserDtoValidation : AbstractValidator<EditUserDto>
    {
        public EditUserDtoValidation()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
                .MinimumLength(3).WithMessage("Tên đăng nhập phải có ít nhất 3 ký tự")
                .MaximumLength(20).WithMessage("Tên đăng nhập tối đa 20 ký tự")
                .Matches("^[a-zA-Z0-9_.-]*$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang");

            RuleFor(u => u.Name)
                .MaximumLength(30).WithMessage("Tên không được vượt quá 30 ký tự");

            RuleFor(u => u.Surname)
                .MaximumLength(30).WithMessage("Họ không được vượt quá 30 ký tự");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Địa chỉ email không hợp lệ");

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại không được để trống")
                .Matches(@"^0\d{9}$").WithMessage("Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số)");
        }
    }
}

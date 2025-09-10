
using LMSCourse.Models;

namespace LMSCourse.DTOs.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string PasswordHash { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string>();

    }
}

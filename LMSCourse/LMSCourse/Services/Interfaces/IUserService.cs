using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        //Task<IEnumerable<UserDto>> GetAllUsersAsync();
        //Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<List<string>> GetRolesNameByIdAsync(int userId);
        Task<List<string>> GetPermissionsNameByIdAsync(int userId);
        Task<User?> GetUserByUserNameOrEmailAsync(string userOrEmail);
        bool VerifyPassword(User user, string password);
    }
}

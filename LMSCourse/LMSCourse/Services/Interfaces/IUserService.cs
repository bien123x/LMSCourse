using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        //Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<User?> RegisterUserAsync(RegisterDto dto);
        Task<List<string>> GetRolesNameByIdAsync(int userId);
        Task<List<string>> GetPermissionsNameByIdAsync(int userId);
        Task<User?> GetUserByUserNameOrEmailAsync(string userOrEmail);
        bool VerifyPassword(User user, string password);
        string HashPasswordUser(User user, string password);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<ViewUserDto>> GetAllViewUser();
        Task<ViewUserDto> AddUserAsync(UserDto userDto);
        Task<ViewUserDto> EditUserDto(int userId, EditUserDto editUserDto);

        Task<List<string>> GetRolesName();
    }
}

using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface IUserService
    {
        Task<ViewUserDto?> GetUserByIdAsync(int id);
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
        Task<bool> DeleteUser(int userId);

        Task<List<string>> GetRolesName();
        Task<List<string>> GetUserPermissionsNameById(int userId);

        Task<List<string>> UpdateUserPermissions(int userId, List<string> permissions);
        Task ResetPassword(int userId, string newPassword);
    }
}

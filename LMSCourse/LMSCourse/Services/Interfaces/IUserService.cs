using LMSCourse.DTOs;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface IUserService
    {
        Task<ViewUserDto?> GetUserByIdAsync(int id);
        //Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<ApiResponse<User>> RegisterUserAsync(RegisterDto dto);
        Task<List<string>> GetRolesNameByIdAsync(int userId);
        Task<List<string>> GetPermissionsCodeByIdAsync(int userId);
        Task<User?> GetUserByUserNameOrEmailAsync(string userOrEmail);
        bool VerifyPassword(User user, string password);
        string HashPasswordUser(User user, string password);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<ViewUserDto>> GetAllViewUser();
        Task<ApiResponse<ViewUserDto>> AddUserAsync(UserDto userDto, int addUserId);
        Task<ViewUserDto> EditUserDto(int userId, EditUserDto editUserDto, int editUserId);
        Task<ApiResponse> DeleteUser(int userId);

        Task<List<string>> GetRolesName();
        Task<List<string>> GetUserPermissionsCodeById(int userId);

        Task<List<string>> UpdateUserPermissions(int userId, List<string> permissions);
        Task ResetPassword(int userId, string newPassword);
        Task<PagedResult<ViewUserDto>> GetPagedUsers(QueryDto query);
        Task<User?> VerifyEmailByToken(string tokenEmail);
        Task<ApiResponse<ViewUserDto>> ChangePasswordByIdAsync(int userId, ChangePasswordDto dto);
        Task IncreaseFailAccessCount(int userId);
        Task ResetFailAccessCount(int userId);
        Task SetLockEndTimeAsync(int userId, int lockoutDuration);
        Task<ApiResponse<DateTime?>> LockUserByIdAsync(int userId, DateTime dateEndTime);
        Task<ApiResponse> UnLockUserByIdAsync(int userId);

        Task<ViewUserDto?> UpdatePersonalInfo(int userId, PersonalInfoDto dto);
    }
}

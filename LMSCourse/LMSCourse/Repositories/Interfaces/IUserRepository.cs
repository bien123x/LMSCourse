using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetUserWithRolesAsync(int userId);
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task<User?> GetUserWithRolesAndPermissionsAsync(int userId);
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);

        Task<bool> CheckExistUserNameOrEmail(string usernameOrEmail);

        Task AddUserRoles(UserRole userRole);
        Task<Role> GetRoleByRoleName(string roleName);
        Task<bool> IsExistUserNameOrEmail(string userName, string email);
    }
}

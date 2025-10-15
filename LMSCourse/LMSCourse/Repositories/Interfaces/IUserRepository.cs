using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        Task<bool> IsExistUserNameOrEmail(string userName, string email, int? userId = null);
        Task<bool> IsExistUserName(string userName);
        Task<bool> IsExistEmail(string email);

        Task<IEnumerable<Role>> GetAllRoles();
        Task<User?> GetWithUserPermissions(int userId);
        Task UpdateUserPermissions(UserPermission userPermission);

        Task<IEnumerable<Permission>> GetPermissionsByPermissionsCode(List<string> permissionsCode);
        Task SaveChangesAsync();
        Task<User?> GetWithUserRolesAndUserPermissions(int userId);
        Task<PagedResult<User>> GetPagedUsersAsync(QueryDto query);

        Task<User?> GetByTokenEmailAsync(string tokenEmail);
    }
}

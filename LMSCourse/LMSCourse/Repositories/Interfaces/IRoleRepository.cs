using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllWithUserRolesAsync();
        Task<Role?> GetWithUserRolesAsync(int roleId);
        Task<IEnumerable<Role>> GetAll();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<bool> IsExistRoleName(string roleName);
        Task<int> CountUsersByRoleId(int roleId);
        Task<Role?> GetWithPermissionsAsync(int roleId);
        Task DeleteAllRolePermissionsByRoleId(int roleId);
        Task<IEnumerable<Permission>> GetPermissionsByNamesAsync(List<string> permissionNames);
        Task SaveChangesAsync();
        Task<Role?> GetById(int roleId);
        Task DeleteAllUserRoleById(int roleId);
        Task AddUserRoleAsync(UserRole userRole);
        Task<bool> IsExistUserRole(UserRole userRole);
    }
}

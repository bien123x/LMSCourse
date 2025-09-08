using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllWithUserRolesAsync();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<bool> IsExistRoleName(string roleName);
        //Task<int> CountUsersByRoleId(int roleId);
    }
}

using LMSCourse.DTOs.Role;
using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<ViewRoleDto>?> GetViewRoles();
        Task<ViewRoleDto> CreateRole(RoleDto dto);
        Task<ViewRoleDto> UpdateRole(int roleId, RoleDto dto);
        Task<List<string>> GetPermissionsById(int roleId);
        Task<List<string>> UpdatePermissions(int roleId, List<string> permissions);
        Task<bool> DeleteRole(int roleId);
        Task<int> CountUserByRoleId(int roleId);
        Task<bool> AssignRoleUserDelete(int roleId, int roleIdAssign);
        Task<IEnumerable<Role>> GetRolesMinusRoleId(int roleId);
        Task<List<string>> GetRolesName();
        
    }
}

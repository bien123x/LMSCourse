using LMSCourse.DTOs.Role;

namespace LMSCourse.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<ViewRoleDto>?> GetViewRoles();
        Task<ViewRoleDto> CreateRole(RoleDto dto);
    }
}

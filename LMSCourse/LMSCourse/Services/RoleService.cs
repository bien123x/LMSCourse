using AutoMapper;
using LMSCourse.DTOs.Role;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using System.Data;

namespace LMSCourse.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<bool> AssignRoleUserDelete(int roleId, int roleIdAssign)
        {
            var roles = await _roleRepository.GetWithUserRolesAsync(roleId);

            if (roles == null) return false;
            foreach (var userRole in roles.UserRoles)
            {
                var isExist = await _roleRepository.IsExistUserRole(new UserRole()
                {
                    UserId = userRole.UserId,
                    RoleId = roleIdAssign
                });
                if (isExist) continue;
                await _roleRepository.AddUserRoleAsync(new UserRole()
                {
                    UserId = userRole.UserId,
                    RoleId = roleIdAssign,
                });
            }

            return true;
        }

        public async Task<int> CountUserByRoleId(int roleId)
        {
            var role = await _roleRepository.GetById(roleId);
            if (role == null) 
                return -1;
            return await _roleRepository.CountUsersByRoleId(roleId);
        }

        public async Task<ViewRoleDto> CreateRole(RoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);

            if (await _roleRepository.IsExistRoleName(dto.RoleName))
                return null;
            await _roleRepository.AddAsync(role);

            var viewRoleDto = _mapper.Map<ViewRoleDto>(role);
            viewRoleDto.CountUserRoles = 0;
            return viewRoleDto;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var role = await _roleRepository.GetById(roleId);
            if (role == null) return false;

            var countUser = await _roleRepository.CountUsersByRoleId(roleId);

            if (countUser != 0)
            {
                //Delete User Role
                await _roleRepository.DeleteAllUserRoleById(roleId);
            }
            await _roleRepository.DeleteAllRolePermissionsByRoleId(roleId);
            await _roleRepository.DeleteAsync(role);
            return true;
        }

        public async Task<List<string>> GetPermissionsById(int roleId)
        {
            var role = await _roleRepository.GetWithPermissionsAsync(roleId);

            if (role == null)
                return new List<string>();
            var permissions = role.RolePermissions
                .Select(rp => rp.Permission.PermissionName)
                .ToList();
            return permissions;
        }

        public async Task<IEnumerable<Role>> GetRolesMinusRoleId(int roleId)
        {
            var roles = await _roleRepository.GetAll();

            return roles.Where(r => r.RoleId != roleId);
        }

        public async Task<IEnumerable<ViewRoleDto>?> GetViewRoles() { 
            var roles = await _roleRepository.GetAllWithUserRolesAsync();

            if (roles == null)
                return null;

            var viewRoleDto = roles.Select(r => new ViewRoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                CountUserRoles = r.UserRoles.Count
            });

            return viewRoleDto;
        }

        public async Task<List<string>> UpdatePermissions(int roleId, List<string> permissionNames)
        {
            var roles = await _roleRepository.GetWithPermissionsAsync(roleId);

            if (roles == null)
                return null;


            await _roleRepository.DeleteAllRolePermissionsByRoleId(roleId);

            // Lấy Permission đã có trong DB
            var permissions = await _roleRepository.GetPermissionsByNamesAsync(permissionNames);

            // Thêm RolePermission mới
            foreach (var perm in permissions)
            {
                roles.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = perm.PermissionId
                });
            }

            // Lưu thay đổi
            await _roleRepository.SaveChangesAsync();

            // Trả về danh sách quyền mới
            return roles.RolePermissions.Select(rp => rp.Permission.PermissionName).ToList();
        }

        public async Task<ViewRoleDto> UpdateRole(int roleId, RoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            role.RoleId = roleId;

            if (await _roleRepository.IsExistRoleName(dto.RoleName))
                return null;

            await _roleRepository.UpdateAsync(role);

            var viewRoleDto = _mapper.Map<ViewRoleDto>(role);
            viewRoleDto.CountUserRoles =await _roleRepository.CountUsersByRoleId(role.RoleId);
            return viewRoleDto;
        }
    }
}

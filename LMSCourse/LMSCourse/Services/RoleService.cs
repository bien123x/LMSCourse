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
    }
}

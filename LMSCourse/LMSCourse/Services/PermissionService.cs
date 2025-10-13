using LMSCourse.DTOs.Permission;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;

namespace LMSCourse.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionDto>> GetTreePermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();

            // build tree
            var lookup = permissions.ToLookup(p => p.ParentId);
            List<PermissionDto> Build(int? parentId)
            {
                return lookup[parentId]
                    .Select(p => new PermissionDto
                    {
                        Id = p.PermissionId,
                        Name = p.PermissionName,
                        Code = p.PermissionCode,
                        Children = Build(p.PermissionId)
                    })
                    .ToList();
            }
            return Build(null); // root (ParentId = 0)
        }
    }
}

using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountUsersByRoleId(int roleId)
        {
            return await _context.UserRoles.Where(ur => ur.RoleId == roleId).CountAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetWithPermissionsAsync(int roleId)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

        }

        public async Task<IEnumerable<Role>> GetAllWithUserRolesAsync()
        {
            return await _context.Roles.Include(r => r.UserRoles).ToListAsync();
        }

        public async Task<bool> IsExistRoleName(string roleName)
        {
            return await _context.Roles.AnyAsync(r => r.RoleName == roleName);
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllRolePermissionsByRoleId(int roleId)
        {
            var rolePermissions = _context.RolePermissions
                                  .Where(rp => rp.RoleId == roleId);

            // Xóa tất cả
            _context.RolePermissions.RemoveRange(rolePermissions);

            // Lưu thay đổi bất đồng bộ
            await _context.SaveChangesAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByNamesAsync(List<string> permissionNames)
        {
            return await _context.Permissions
                .Where(p => permissionNames.Contains(p.PermissionName))
                .ToListAsync();
        }

        public async Task<Role?> GetById(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task DeleteAllUserRoleById(int roleId)
        {
            var userRole = await _context.UserRoles.Where(ur => ur.RoleId == roleId).ToListAsync();

            _context.UserRoles.RemoveRange(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetWithUserRolesAsync(int roleId)
        {
            return await _context.Roles
                .Where(r => r.RoleId == roleId)
                .Include(r => r.UserRoles)
                .FirstOrDefaultAsync();
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<bool> IsExistUserRole(UserRole userRole)
        {
            return await _context.UserRoles.AnyAsync(ur => ur.RoleId == userRole.RoleId && ur.UserId == userRole.UserId);
        }
    }
}

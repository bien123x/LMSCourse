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

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
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
    }
}

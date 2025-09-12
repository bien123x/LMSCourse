using LMSCourse.Data;
using LMSCourse.DTOs.Page;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace LMSCourse.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserWithRolesAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserWithRolesAndPermissionsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task<bool> CheckExistUserNameOrEmail(string usernameOrEmail)
        {
            return await _context.Users.AnyAsync(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task<IEnumerable<User>> GetAllWithRolesAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task AddUserRoles(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> GetRoleByRoleName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<bool> IsExistUserNameOrEmail(string userName, string email, int? userId = null)
        {
            return await _context.Users.AnyAsync(u => (u.UserName == userName || u.Email == email) && 
            (!userId.HasValue || u.UserId != userId)
            );
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<User?> GetWithUserPermissions(int userId)
        {
            return await _context.Users
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateUserPermissions(UserPermission userPermission)
        {
            await _context.UserPermissions.AddAsync(userPermission);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByPermissionsName(List<string> permissionsName)
        {
            return await _context.Permissions.Where(p => permissionsName.Contains(p.PermissionName)).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetWithUserRolesAndUserPermissions(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<LMSCourse.DTOs.Page.PagedResult<User>> GetPagedUsersAsync(QueryDto query)
        {
            var users = _context.Users
                                .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                                .AsQueryable();

            foreach (var filter in query.Filters)
            {

                if (string.IsNullOrWhiteSpace(filter.Value)) continue;

                switch (filter.Field.ToLower())
                {
                    case "username":
                        users = users.Where(u => u.UserName.Contains(filter.Value));
                        break;
                    case "email":
                        users = users.Where(u => u.Email.Contains(filter.Value));
                        break;
                    case "role":
                        users = users.Where(u => u.UserRoles.Select(ur => ur.Role.RoleName).Contains(filter.Value));
                        break;
                }
            }

            // --- total count trước khi paging ---
            var totalCount = await users.CountAsync();

            if (query.Sorts != null && query.Sorts.Count > 0)
            {
                // Tạo chuỗi sort để System.Linq.Dynamic.Core đọc được
                // Ví dụ: "UserName asc, Email desc"
                var sortString = string.Join(", ", query.Sorts.Select(s => $"{s.Field} {s.Order}"));
                users = users.OrderBy(sortString); // OrderBy của System.Linq.Dynamic.Core
            }

            // --- pagination ---
            var items = await users
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new LMSCourse.DTOs.Page.PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}

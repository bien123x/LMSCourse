using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDbContext _context;
        public SettingsRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<PasswordPolicy> GetPolicyAsync()
        {
            return await _context.PasswordPolicies.FirstAsync();
        }

        public async Task UpdatePolicyAsync(PasswordPolicy policy)
        {
            _context.PasswordPolicies.Update(policy);
            await _context.SaveChangesAsync();
        }
    }
}

using LMSCourse.Data;
using LMSCourse.DTOs.Setting;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;
        public SettingRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<IdentitySetting> GetIdentityAsync()
        {
            return await _context.IdentitySettings.FirstAsync();
        }

        public async Task<PasswordPolicy> GetPolicyAsync()
        {
            return await _context.PasswordPolicies.FirstAsync();
        }

        public async Task UpdateIdentityAsync(IdentitySetting identitySetting)
        {
            _context.IdentitySettings.Update(identitySetting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePolicyAsync(PasswordPolicy policy)
        {
            _context.PasswordPolicies.Update(policy);
            await _context.SaveChangesAsync();
        }
    }
}

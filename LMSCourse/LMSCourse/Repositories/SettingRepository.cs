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

       

        public async Task UpdateIdentityAsync(IdentitySetting identitySetting)
        {
            _context.IdentitySettings.Update(identitySetting);
            await _context.SaveChangesAsync();
        }

       
    }
}

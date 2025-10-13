using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Repositories
{
    public class CertificateTemplateRepository : GenericRepository<CertificateTemplate>, ICertificateTemplateRepository
    {
        public CertificateTemplateRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CertificateTemplate?> GetStatusIsActiveByTeacherId(int teacherId)
        {
            return await _context.CertificateTemplates
                .Where(ct => ct.TeacherId == teacherId && ct.IsActive == true)
                .FirstOrDefaultAsync();
        }
    }
}

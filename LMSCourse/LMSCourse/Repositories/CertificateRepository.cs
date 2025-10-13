using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;

namespace LMSCourse.Repositories
{
    public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Certificate> GetAllQuery()
        {
            return _context.Certificates.AsQueryable();
        }
    }
}

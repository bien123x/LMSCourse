using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ICertificateRepository : IGenericRepository<Certificate>
    {
        IQueryable<Certificate> GetAllQuery();
    }
}

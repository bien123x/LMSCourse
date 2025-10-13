using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ICertificateTemplateRepository : IGenericRepository<CertificateTemplate>
    {
        Task<CertificateTemplate?> GetStatusIsActiveByTeacherId(int teacherId);
    }
}

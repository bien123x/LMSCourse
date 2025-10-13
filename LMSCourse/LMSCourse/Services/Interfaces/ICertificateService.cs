using LMSCourse.DTOs.Certificate;

namespace LMSCourse.Services.Interfaces
{
    public interface ICertificateService
    {
        Task CreateCertificate(CertificateDto dto);

        Task<IEnumerable<CertificateViewDto>?> GetCertificatesByUserId(int userId);
    }
}

using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;

namespace LMSCourse.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddLogAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<PagedResult<AuditLog>> GetAllByQueryAsync(QueryDto query);
        Task<List<int>> GetDistinctStatusCode();
        Task<List<string>> GetDistinctHttpMethod();

    }
}
using LMSCourse.Dtos;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;

namespace LMSCourse.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddLogAsync(AuditLog log);
        //Task<PagedResult<AuditLog>> SearchLogsAsync(AuditLogSearchDto searchDto);
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<PagedResult<AuditLog>> GetAllByQueryAsync(QueryDto query);

    }
}
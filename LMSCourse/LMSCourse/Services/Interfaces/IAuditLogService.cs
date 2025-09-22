using LMSCourse.Dtos;
using LMSCourse.DTOs.Page;
using LMSCourse.DTOs.Page_Sort_Filter;

namespace LMSCourse.Interfaces
{
    public interface IAuditLogService
    {
        // Ghi một bản ghi log mới.
        Task CreateLogAsync(AuditLogDto dto);

        // Tìm kiếm các bản ghi log dựa trên các tiêu chí.
        // Trả về một đối tượng phân trang chứa danh sách các log đã được ánh xạ thành DTO.
        //Task<PagedResult<AuditLogDto>> SearchLogsAsync(AuditLogSearchDto searchDto);

        Task<IEnumerable<AuditLogDto>> GetAllAuditLogsAsync();

        Task<PagedResult<AuditLogDto>> GetAllAuditLogsByQueryAsync(QueryDto queryDto);
    }
}
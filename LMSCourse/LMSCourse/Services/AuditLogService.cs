using AutoMapper;
using LMSCourse.Dtos;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Interfaces;
using LMSCourse.Models;

namespace LMSCourse.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _logRepository;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task CreateLogAsync(AuditLogDto dto)
        {
            // Ánh xạ DTO sang Entity trước khi lưu vào database
            var logEntity = new AuditLog
            {
                HttpMethod = dto.HttpMethod,
                Url = dto.Url,
                UserId = dto.UserId,
                StatusCode = dto.StatusCode,
                UserName = dto.UserName,
                IpAddress = dto.IpAddress,
                Duration = dto.Duration,
                BrowserInfo = dto.BrowserInfo,
                Exception = dto.Exception,
                // Thêm các thuộc tính khác nếu cần thiết
                ApplicationName = "LMSCourse",
                CreatedAt = DateTime.UtcNow
            };

            await _logRepository.AddLogAsync(logEntity);
        }

        public async Task<IEnumerable<AuditLogDto>> GetAllAuditLogsAsync()
        {
            var auditLogs = await _logRepository.GetAllAsync();
            var auditLogDtos = _mapper.Map<List<AuditLogDto>>(auditLogs);
            return auditLogDtos;
        }

        public async Task<PagedResult<AuditLogDto>> GetAllAuditLogsByQueryAsync(QueryDto queryDto)
        {
            var auditLogsDto = await _logRepository.GetAllByQueryAsync(queryDto);

            return new PagedResult<AuditLogDto>
            {
                Items = _mapper.Map<IEnumerable<AuditLogDto>>(auditLogsDto.Items),
                TotalCount = auditLogsDto.TotalCount
            };
        }

        //public async Task<PagedResult<AuditLogDto>> SearchLogsAsync(AuditLogSearchDto searchDto)
        //{
        //    // Gọi Repository để lấy dữ liệu đã được tìm kiếm và phân trang
        //    var pagedLogs = await _logRepository.SearchLogsAsync(searchDto);

        //    // Ánh xạ danh sách Entity (AuditLog) sang danh sách DTO (AuditLogDto)
        //    var logDtos = pagedLogs.Items.Select(log => new AuditLogDto
        //    {
        //        HttpMethod = log.HttpMethod,
        //        Url = log.Url,
        //        StatusCode = log.StatusCode,
        //        UserName = log.UserName,
        //        IpAddress = log.IpAddress,
        //        Duration = log.Duration,
        //        BrowserInfo = log.BrowserInfo,
        //        Exception = log.Exception,
        //        CreatedAt = log.CreatedAt
        //    }).ToList();

        //    // Trả về kết quả đã được phân trang và ánh xạ
        //    return new PagedResult<AuditLogDto>
        //    {
        //        Items = logDtos,
        //        TotalCount = pagedLogs.TotalCount,
        //    };
        //}
    }
}
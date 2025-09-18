using LMSCourse.Dtos;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        /// <summary>
        /// Tạo một bản ghi log mới.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuditLogDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Log data is required.");
            }

            await _auditLogService.CreateLogAsync(dto);
            return Ok(new { message = "Log created successfully" });
        }

        /// <summary>
        /// Tìm kiếm, lọc, phân trang các log.
        /// </summary>
        //[HttpGet("search")]
        //public async Task<IActionResult> Search([FromQuery] AuditLogSearchDto searchDto)
        //{
        //    var result = await _auditLogService.SearchLogsAsync(searchDto);
        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllAudiLogsAsync()
        {
            var auditlogs = await _auditLogService.GetAllAuditLogsAsync();

            return Ok(auditlogs);
        }

        [HttpPost("audit-logs")]
        public async Task<IActionResult> GetAllAuditLogsByQuery([FromBody] QueryDto query)
        {
            var result = await _auditLogService.GetAllAuditLogsByQueryAsync(query);

            return Ok(result);
        }
    }
}

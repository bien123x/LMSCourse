using LMSCourse.Data;
using LMSCourse.Dtos;
using LMSCourse.DTOs.Page;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Interfaces;
using LMSCourse.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace LMSCourse.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<LMSCourse.DTOs.Page.PagedResult<AuditLog>> GetAllByQueryAsync(QueryDto query)
        {
            var auditLogs = _context.AuditLogs.OrderByDescending(a => a.CreatedAt).AsQueryable().AsNoTracking();

            foreach (var filter in query.Filters)
            {
                if (string.IsNullOrWhiteSpace(filter.Value)) continue;

                switch (filter.Field.ToLower())
                {
                    case "httpmethod":
                        auditLogs = auditLogs.Where(a => a.HttpMethod.Equals(filter.Value));
                        break;
                    case "statuscode":
                        if (int.TryParse(filter.Value, out var code))
                        {
                            auditLogs = auditLogs.Where(a => a.StatusCode == code);
                        }
                        break ;
                    case "global":
                        auditLogs = auditLogs.Where(a => a.UserName.Contains(filter.Value) || a.HttpMethod.Contains(filter.Value));
                        break;
                    case "createdat":
                        var parts = filter.Value.Split('*', StringSplitOptions.TrimEntries);
                        var startDate = DateTime.Parse(parts[0]); // ngày bắt đầu
                        var endDate = DateTime.Parse(parts[1]).AddDays(1).AddTicks(-1); // cuối ngày
                        var time = auditLogs.FirstOrDefault(a => a.AuditLogId == 709).CreatedAt;
                        auditLogs = auditLogs
                            .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate);
                        break;

                }

            }

            var totalCount = await auditLogs.CountAsync();

            if (query.Sorts != null && query.Sorts.Count > 0)
            {
                var sortString = string.Join(", ", query.Sorts.Select(s => $"{s.Field} {s.Order}"));
                auditLogs = auditLogs.OrderBy(sortString);
            }

            var items = await auditLogs
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            

            return new DTOs.Page.PagedResult<AuditLog>
            {
                Items = items,
                TotalCount = totalCount,
            };
        }

        //public async Task<LMSCourse.DTOs.Page.PagedResult<AuditLogDto>> SearchLogsAsync(AuditLogSearchDto searchDto)
        //{
        //    var query = _context.AuditLogs.AsQueryable();

        //    // Áp dụng các bộ lọc cụ thể từ AuditLogSearchDto
        //    if (!string.IsNullOrEmpty(searchDto.Keyword))
        //    {
        //        query = query.Where(l => l.Url!.Contains(searchDto.Keyword) ||
        //                                 l.UserName!.Contains(searchDto.Keyword) ||
        //                                 l.HttpMethod!.Contains(searchDto.Keyword) ||
        //                                 (l.Exception != null && l.Exception.Contains(searchDto.Keyword)) ||
        //                                 l.IpAddress!.Contains(searchDto.Keyword));
        //    }

        //    if (!string.IsNullOrEmpty(searchDto.UserName))
        //    {
        //        query = query.Where(l => l.UserName == searchDto.UserName);
        //    }

        //    if (!string.IsNullOrEmpty(searchDto.IpAddress))
        //    {
        //        query = query.Where(l => l.IpAddress == searchDto.IpAddress);
        //    }

        //    if (searchDto.StatusCode.HasValue)
        //    {
        //        query = query.Where(l => l.StatusCode == searchDto.StatusCode.Value);
        //    }

        //    if (searchDto.StartDate.HasValue)
        //    {
        //        query = query.Where(l => l.CreatedAt >= searchDto.StartDate.Value);
        //    }

        //    if (searchDto.EndDate.HasValue)
        //    {
        //        // Thêm 1 ngày để bao gồm cả thời gian cuối của ngày EndDate
        //        var endOfDay = searchDto.EndDate.Value.AddDays(1);
        //        query = query.Where(l => l.CreatedAt < endOfDay);
        //    }

        //    // Lấy tổng số bản ghi trước khi phân trang
        //    var totalCount = await query.CountAsync();

        //    // Xử lý sắp xếp động dựa trên SortBy và SortOrder
        //    var property = typeof(AuditLog).GetProperty(searchDto.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        //    if (property != null)
        //    {
        //        var parameter = Expression.Parameter(typeof(AuditLog), "l");
        //        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        //        var orderByExp = Expression.Lambda(propertyAccess, parameter);
        //        string methodName = searchDto.SortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

        //        var resultExp = Expression.Call(typeof(Queryable), methodName,
        //                                        new Type[] { typeof(AuditLog), property.PropertyType },
        //                                        query.Expression, Expression.Quote(orderByExp));
        //        query = query.Provider.CreateQuery<AuditLog>(resultExp);
        //    }
        //    else
        //    {
        //        // Mặc định sắp xếp theo CreatedAt giảm dần nếu không có sắp xếp nào được chỉ định
        //        query = query.OrderByDescending(l => l.CreatedAt);
        //    }

        //    // Áp dụng phân trang
        //    var items = await query.Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
        //                           .Take(searchDto.PageSize)
        //                           .ToListAsync();

        //    return new LMSCourse.DTOs.Page.PagedResult<AuditLogDto>
        //    {
        //        Items = items,
        //        TotalCount = totalCount,

        //    };
    //}
    }
}
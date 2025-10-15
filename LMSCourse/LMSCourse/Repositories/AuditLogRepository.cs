using LMSCourse.Data;
using LMSCourse.Dtos;
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
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<DTOs.Page_Sort_Filter.PagedResult<AuditLog>> GetAllByQueryAsync(QueryDto query)
        {
            var auditLogs = _context.AuditLogs.AsQueryable().AsNoTracking();

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
                        auditLogs = auditLogs.Where(a => a.UserName.Contains(filter.Value) || a.HttpMethod.Contains(filter.Value) || a.Url.Contains(filter.Value));
                        break;
                    case "createdat":
                        var parts = filter.Value.Split('*', StringSplitOptions.TrimEntries);
                        var startDate = DateTime.Parse(parts[0]); // ngày bắt đầu
                        var endDate = DateTime.Parse(parts[1]).AddDays(1).AddTicks(-1); // cuối ngày
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
            } else
            {
                auditLogs = auditLogs.OrderByDescending(a => a.CreatedAt);
            }

            var items = await auditLogs
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            

            return new DTOs.Page_Sort_Filter.PagedResult<AuditLog>
            {
                Items = items,
                TotalCount = totalCount,
            };
        }

        public async Task<List<string>> GetDistinctHttpMethod()
        {
            try
            {
                await _context.AuditLogs.Select(a => a.HttpMethod).Distinct().ToListAsync();
            } catch (Exception ex)
            {

            } finally
            {
            }
            return await _context.AuditLogs.Select(a => a.HttpMethod).Distinct().ToListAsync();
        }

        public async Task<List<int>> GetDistinctStatusCode()
        {
            return await _context.AuditLogs.Select(a => a.StatusCode).Distinct().ToListAsync();
        }

        
    }
}
using LMSCourse.Dtos;
using LMSCourse.Interfaces;
using System.Diagnostics;
using System.Security.Claims;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAuditLogService auditLogService)
    {


        var stopwatch = Stopwatch.StartNew();
        var logDto = new AuditLogDto();

        if (context.Request.Method == HttpMethods.Get)
        {
            await _next(context);
            return;
        }

        var path = context.Request.Path.ToString().ToLower();

        // Ví dụ bỏ qua log khi gọi API lấy danh sách audit log
        if ((path.Contains("/auditlogs/audit-logs") ||path.Contains("/user/users")) && context.Request.Method == "POST")
        {
            await _next(context);
            return;
        }

        try
        {
            logDto.HttpMethod = context.Request.Method;
            logDto.Url = context.Request.Path;
            logDto.IpAddress = context.Connection.RemoteIpAddress?.ToString();
            logDto.BrowserInfo = context.Request.Headers["User-Agent"].ToString();

            if (context.User.Identity?.IsAuthenticated == true)
            {
                logDto.UserId = int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                logDto.UserName = context.User.Identity.Name;
            }

            await _next(context);

            logDto.StatusCode = context.Response.StatusCode;
        }
        catch (Exception ex)
        {
            logDto.Exception = ex.Message;
            logDto.StatusCode = 500;
        }
        finally
        {
            stopwatch.Stop();
            logDto.Duration = stopwatch.ElapsedMilliseconds;
            await auditLogService.CreateLogAsync(logDto);
        }
    }
}
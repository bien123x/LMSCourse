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

        var path = context.Request.Path.ToString().ToLower();

        // Bỏ qua các GET hoặc API log/không cần log
        if (context.Request.Method == HttpMethods.Get ||
            (path.Contains("/auditlogs/audit-logs") && context.Request.Method == "POST") ||
            (path.Contains("/user/users") && context.Request.Method == "POST"))
        {
            await _next(context);
            return;
        }

        try
        {
            // Thông tin cơ bản
            logDto.HttpMethod = context.Request.Method;
            logDto.Url = context.Request.Path;
            logDto.IpAddress = context.Connection.RemoteIpAddress?.ToString();
            logDto.BrowserInfo = context.Request.Headers["User-Agent"].ToString();

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
                {
                    logDto.UserId = userId;
                }
                logDto.UserName = context.User.Identity.Name;
            }

            // Xử lý request
            await _next(context);

            // Chỉ đọc status code, không set
            logDto.StatusCode = context.Response.StatusCode;
        }
        catch (Exception ex)
        {
            // Lỗi middleware / exception
            logDto.Exception = ex.Message;

            // Chỉ set StatusCode nếu response chưa bắt đầu
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "Internal Server Error" });
            }
        }
        finally
        {
            stopwatch.Stop();
            logDto.Duration = stopwatch.ElapsedMilliseconds;

            // Ghi log async, không block response
            try
            {
                await auditLogService.CreateLogAsync(logDto);
            }
            catch
            {
                // Không ném exception nếu log thất bại
            }
        }
    }
}

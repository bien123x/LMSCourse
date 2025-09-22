namespace LMSCourse.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? HttpMethod { get; set; }
        public string? Url { get; set; }
        public int StatusCode { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public string? ApplicationName { get; set; }
        public long Duration { get; set; } // milliseconds

        public string? BrowserInfo { get; set; }
        public string? Exception { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
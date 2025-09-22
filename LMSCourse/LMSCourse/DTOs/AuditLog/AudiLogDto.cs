namespace LMSCourse.Dtos
{
    public class AuditLogDto
    {
        public int? UserId { get; set; }
        public string? HttpMethod { get; set; }
        public string? Url { get; set; }
        public int StatusCode { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public long Duration { get; set; }
        public string? BrowserInfo { get; set; }
        public string? Exception { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
namespace LMSCourse.Dtos
{
    public class AuditLogSearchDto
    {
        public string? Keyword { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Bổ sung các thuộc tính phân trang và sắp xếp
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
    }
}
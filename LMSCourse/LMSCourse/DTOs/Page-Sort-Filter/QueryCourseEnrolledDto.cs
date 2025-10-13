namespace LMSCourse.DTOs.Page_Sort_Filter
{
    public class QueryCourseEnrolledDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortField { get; set; }    // ví dụ: "price"
        public int SortOrder { get; set; } = 1;   // 1 = ASC, -1 = DESC
        public string Status { get; set; } = "Active";
    }
}

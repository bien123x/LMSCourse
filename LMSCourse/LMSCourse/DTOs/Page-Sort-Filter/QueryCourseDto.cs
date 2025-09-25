namespace LMSCourse.DTOs.Page_Sort_Filter
{
    public class QueryCourseDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortField { get; set; }    // ví dụ: "price"
        public int SortOrder { get; set; } = 1;   // 1 = ASC, -1 = DESC
        public CourseFilterRequest? CourseFilterRequest { get; set; }
    }

    public class CourseFilterRequest
    {
        public List<int>? TeacherIds { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? LevelIds { get; set; }
        public List<int>? LanguageIds { get; set; }
    }
}

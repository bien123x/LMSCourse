namespace LMSCourse.DTOs.Page_Sort_Filter
{
    public class QueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public List<SortField> Sorts { get; set; } = new();
        public List<FilterField> Filters { get; set; } = new();
    }
}

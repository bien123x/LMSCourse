using LMSCourse.DTOs;
using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseCreateUpdateDto> CreateAsync(CourseCreateUpdateDto courseCreateUpdateDto);
        Task<CourseFiltersDto> GetCourseFilterAsync();
        Task<PagedResult<CourseDto>> GetAllWithFilter(QueryCourseDto dto);
        Task<ApiResponse<CourseDto>> GetCourseByIdAsync(int courseId);

    }
}

using LMSCourse.DTOs;
using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync(int userId);
        Task<IEnumerable<CourseDto>> GetCoursesByListId(List<int> courseIds, int userId);
        Task<CourseCreateUpdateDto> CreateAsync(CourseCreateUpdateDto courseCreateUpdateDto);
        Task<CourseFiltersDto> GetCourseFilterAsync(int userId);
        Task<PagedResult<CourseDto>> GetAllWithFilter(QueryCourseDto dto, int userId);
        Task<ApiResponse<CourseDto>> GetCourseByIdAsync(int courseId);
        Task<PagedResult<CourseDto>> GetCoursesEnrolled(int userId, QueryCourseEnrolledDto dto);
        Task<int?> GetRemainingCapacity(int courseId);
    }
}

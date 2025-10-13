using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllWithDataDto(int userId);
        Task<PagedResult<Course>> GetAllWithFilters(QueryCourseDto dto, int userId);
        Task<Course?> GetWithDataDtoByIdAsync(int id);
        Task<PagedResult<Course>> GetAllWithEnrolledDataDto(int userId, QueryCourseEnrolledDto dto);
        Task<int?> GetRemainingCapacity(int courseId);
    }
}

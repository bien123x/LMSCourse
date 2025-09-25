using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllWithDataDto();
        Task<PagedResult<Course>> GetAllWithFilters(QueryCourseDto dto);
        Task<Course?> GetWithDataDtoByIdAsync(int id);
    }
}

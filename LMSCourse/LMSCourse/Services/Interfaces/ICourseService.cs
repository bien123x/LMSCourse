using LMSCourse.DTOs.Course;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseCreateUpdateDto> CreateAsync(CourseCreateUpdateDto courseCreateUpdateDto);
    }
}

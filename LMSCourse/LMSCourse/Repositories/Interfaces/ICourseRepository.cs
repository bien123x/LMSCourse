using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllWithDataDto();  
    }
}

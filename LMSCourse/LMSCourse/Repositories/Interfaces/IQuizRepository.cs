
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IQuizRepository : IGenericRepository<Quiz>
    {
        Task<IEnumerable<Quiz>?> GetQuizzesByCourseId(int courseId);
        Task<int> GetTotalMarksByCourseId(int courseId);
        Task<int> GetTotalQuizOfCourse(int courseId);

    }
}

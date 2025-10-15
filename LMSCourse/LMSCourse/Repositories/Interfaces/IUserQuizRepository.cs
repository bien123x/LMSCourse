using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IUserQuizRepository : IGenericRepository<UserQuiz>
    {
        Task<UserQuiz?> GetWithQuizById(int userQuizId);
        IQueryable<UserQuiz> GetAllQuery();
        Task<int> GetMarkByCourseId(int courseId, int userId);
        Task<UserQuiz?> GetUserQuizHasMaxScore(int quizId, int userId);
    }
}

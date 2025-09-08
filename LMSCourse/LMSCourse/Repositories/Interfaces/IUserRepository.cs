using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetUserWithRolesAsync(int userId);
        Task<User?> GetUserWithRolesAndPermissionsAsync(int userId);
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);

        Task<bool> CheckExistUserNameOrEmail(string usernameOrEmail);
    }
}

using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface IPermissionsRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
    }
}

using LMSCourse.Data;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        Task<PasswordPolicy> GetPolicyAsync();
        Task UpdatePolicyAsync(PasswordPolicy policy);
    }
}

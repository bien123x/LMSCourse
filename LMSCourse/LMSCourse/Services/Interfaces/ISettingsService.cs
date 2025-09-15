using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<(bool IsValid, List<string> Errors)> ValidateAsync(string password);
        Task<PasswordPolicy?> GetPasswordPolicy();
        Task<PasswordPolicy> UpdatePasswordPolicy(PasswordPolicy pwdPolicyDto);
    }
}

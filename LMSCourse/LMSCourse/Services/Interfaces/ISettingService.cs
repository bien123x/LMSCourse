using LMSCourse.DTOs;
using LMSCourse.DTOs.Setting;
using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Services.Interfaces
{
    public interface ISettingService
    {
        Task<(bool IsValid, List<string> Errors)> ValidateAsync(string password);

        Task<IdentitySettingDto> GetIdentitySettingAsync();

        Task<IdentitySettingDto?> UpdateIdentitySettingAsync(IdentitySettingDto identitySettingDto);
        Task<bool> IsPasswordExpiration(DateTime lastChangePassword);
        Task<ApiResponse<LockoutAccountDto>> IsLockOutAsync(DateTime? lockoutEndTime, int failedAccessCount);

        Task<UserSettingDto> GetUserSettingAsync();
    }
}

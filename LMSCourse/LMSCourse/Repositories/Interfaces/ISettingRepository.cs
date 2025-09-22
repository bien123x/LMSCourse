using LMSCourse.Data;
using LMSCourse.DTOs.Setting;
using LMSCourse.Models;

namespace LMSCourse.Repositories.Interfaces
{
    public interface ISettingRepository
    {

        Task<IdentitySetting> GetIdentityAsync();
        Task UpdateIdentityAsync(IdentitySetting identitySetting);
    }
}

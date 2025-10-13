using LMSCourse.DTOs.PaymentDto;
using LMSCourse.DTOs.Permission;

namespace LMSCourse.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetTreePermissionsAsync();
    }
}

namespace LMSCourse.DTOs.User
{
    public class UserPermissionsDto
    {
        public List<string> UserPermissions { get; set; } = new List<string>();
        public List<string> RolePermissions { get; set; } = new List<string>();
    }
}

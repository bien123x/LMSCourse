namespace LMSCourse.DTOs.Role
{
    public class ViewRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = default!;
        public int CountUserRoles { get; set; }
    }
}

namespace LMSCourse.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionCode {  get; set; } = string.Empty;
        public int? ParentId { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}

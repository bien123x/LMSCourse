namespace LMSCourse.Models
{
    public class RolePermission
    {
        public int RolePermissionId { get; set; } // Primary Key
        public int RoleId { get; set; }
        public Role? Role { get; set; } // Navigation property
        public int PermissionId { get; set; }
        public Permission? Permission { get; set; } // Navigation property

    }
}

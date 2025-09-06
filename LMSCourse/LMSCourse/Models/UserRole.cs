namespace LMSCourse.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; } // Primary Key
        public int UserId { get; set; }
        public User? User { get; set; } 
        public int RoleId { get; set; }
        public Role? Role { get; set; } 
    }
}

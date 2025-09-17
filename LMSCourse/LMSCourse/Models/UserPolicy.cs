namespace LMSCourse.Models
{
    public class UserPolicy
    {
        public int Id { get; set; }
        public bool IsUserNameUpdateEnabled { get; set; }
        public bool IsEmailUpdateEnabled { get; set; }
    }
}

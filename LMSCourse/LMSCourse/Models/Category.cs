namespace LMSCourse.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Quan hệ
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

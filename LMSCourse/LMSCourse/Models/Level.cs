namespace LMSCourse.Models
{
    public class Level
    {
        public int LevelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

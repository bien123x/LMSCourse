namespace LMSCourse.Models
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

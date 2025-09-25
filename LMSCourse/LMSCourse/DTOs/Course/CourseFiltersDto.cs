using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.DTOs.Course
{
    public class CourseFiltersDto
    {
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public List<TeacherDto> Teachers { get; set; } = new List<TeacherDto>();
        public List<LevelDto> Levels { get; set; } = new List<LevelDto>();
        public List<LanguageDto> Languages { get; set; } = new List<LanguageDto>();

    }
    public class CategoryDto {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }

    }
    public class TeacherDto
    {
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class LevelDto
    {
        public int LevelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class LanguageDto
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}

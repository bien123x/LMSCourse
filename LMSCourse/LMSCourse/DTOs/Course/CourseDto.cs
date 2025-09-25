using LMSCourse.Models;

namespace LMSCourse.DTOs.Course
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public int? MaxStudents { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Teacher info
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }

        // Category info
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Level info
        public int LevelId { get; set; }
        public string? LevelName { get; set; }

        // Language info
        public int LanguageId { get; set; }
        public string? LanguageName { get; set; }

        // FAQ Groups
        public List<FaqGroupDto> FaqGroups { get; set; } = new List<FaqGroupDto>();

        // Course Topics
        public List<CourseTopicDto> CourseTopics { get; set; } = new List<CourseTopicDto>();

        // Media
        public string? ThumbnailUrl { get; set; }
        public string? VideoType { get; set; }
        public string? VideoUrl { get; set; }

        // Pricing
        public bool IsFree { get; set; }
        public long? Price { get; set; }
        public bool HasDiscount { get; set; }
        public long? DiscountPrice { get; set; }
        public bool IsLifetime { get; set; }
        public int? DurationInMonths { get; set; }
    }
    public class CourseTopicDto
    {
        public int CourseTopicId { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<LessonDto> Lessons { get; set; } = new List<LessonDto>();
    }
    public class LessonDto
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
    }
    public class FaqGroupDto
    {
        public int FaqGroupId { get; set; }
        public string Title { get; set; } = string.Empty;

        // Các FAQ items
        public List<FaqItemDto> FaqItems { get; set; } = new List<FaqItemDto>();
    }
    public class FaqItemDto
    {
        public int FaqItemId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}

namespace LMSCourse.DTOs.Course
{
    public class CourseCreateUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true;
        public int? MaxStudents { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }

        public int TeacherId { get; set; }
        public int CategoryId { get; set; }
        public int LevelId { get; set; }
        public int LanguageId { get; set; }

        public string? ThumbnailUrl { get; set; }
        public string? VideoType { get; set; }
        public string? VideoUrl { get; set; }

        public bool IsFree { get; set; } = false;
        public long? Price { get; set; }
        public bool HasDiscount { get; set; } = false;
        public long? DiscountPrice { get; set; }
        public bool IsLifetime { get; set; } = true;
        public int? DurationInMonths { get; set; }

        // ===== Thêm FAQ Groups, Topics và Lessons =====
        public List<FaqGroupCreateUpdateDto>? FaqGroups { get; set; }
        public List<CourseTopicCreateUpdateDto>? CourseTopics { get; set; }
    }

    // Lesson DTO
    public class LessonCreateUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? LessonContent { get; set; }      // Nội dung bài học
        public string? Description { get; set; }
        public bool IsFreeOrPremium { get; set; } = true;
    }

    // CourseTopic DTO
    public class CourseTopicCreateUpdateDto
    {
        public string Title { get; set; } = string.Empty;

        // Danh sách Lesson trong Topic
        public List<LessonCreateUpdateDto>? Lessons { get; set; }
    }

    // FaqItem DTO
    public class FaqItemCreateUpdateDto
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }

    // FaqGroup DTO
    public class FaqGroupCreateUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public List<FaqItemCreateUpdateDto> FaqItems { get; set; } = new List<FaqItemCreateUpdateDto>();
    }
}

namespace LMSCourse.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;

        // Public / Private
        public bool IsPublic { get; set; } = true;

        // Số lượng học viên tối đa
        public int? MaxStudents { get; set; }

        // Mô tả ngắn và chi tiết
        public string? ShortDescription { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        // Ngày tạo & cập nhật
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Liên kết giáo viên
        public int TeacherId { get; set; }
        public User? Teacher { get; set; }

        // Liên kết danh mục
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Liên kết cấp độ
        public int LevelId { get; set; }
        public Level? Level { get; set; }

        // Liên kết ngôn ngữ
        public int LanguageId { get; set; }
        public Language? Language { get; set; }

        // FAQ
        public ICollection<FaqGroup> FaqGroups { get; set; } = new List<FaqGroup>();

        // CourseTopic
        public ICollection<CourseTopic> CourseTopics { get; set; } = new List<CourseTopic>();

        // ===== Course Media =====
        public string? ThumbnailUrl { get; set; }    // link ảnh thumbnail
        public string? VideoType { get; set; }       // ExternalURL, YouTube, Vimeo, Upload
        public string? VideoUrl { get; set; }        // Link video / file path

        // ===== Pricing =====
        public bool IsFree { get; set; } = false;           // khóa học miễn phí
        public long? Price { get; set; }                 // giá gốc
        public bool HasDiscount { get; set; } = false;      // có giảm giá không
        public long? DiscountPrice { get; set; }         // giá giảm

        public bool IsLifetime { get; set; } = true;        // true = lifetime, false = limited
        public int? DurationInMonths { get; set; }          // số tháng truy cập nếu limited
    }
}

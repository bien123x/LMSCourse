namespace LMSCourse.DTOs.NewFolder
{
    public abstract class QuestionBase<T>
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool Required { get; set; } = false;
        public int Order { get; set; } = 1;
        public string ControlType { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Ví dụ: "text", "email", ...
        public T? Value { get; set; }
    }
}

using LMSCourse.DTOs.NewFolder;

namespace LMSCourse.DTOs.DynamicForm
{
    public class DropdownQuestion : QuestionBase<string>
    {
        public List<OptionItem> Options { get; set; } = new List<OptionItem>();

        public DropdownQuestion()
        {
            ControlType = "dropdown";
        }
    }

    public class OptionItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}

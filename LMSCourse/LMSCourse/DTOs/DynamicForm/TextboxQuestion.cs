using LMSCourse.DTOs.NewFolder;

namespace LMSCourse.DTOs.DynamicForm
{
    public class TextboxQuestion : QuestionBase<string>
    {
        public TextboxQuestion()
        {
            ControlType = "textbox";
        }
    }
}

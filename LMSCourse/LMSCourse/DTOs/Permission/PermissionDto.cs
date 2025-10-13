namespace LMSCourse.DTOs.Permission
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<PermissionDto> Children { get; set; } = new();
    }
}

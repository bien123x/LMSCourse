namespace LMSCourse.Models
{
    public class PasswordPolicy
    {
        public int PasswordPolicyId { get; set; }
        public int MinLength { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
    }
}

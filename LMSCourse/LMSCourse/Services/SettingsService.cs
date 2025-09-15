
using AutoMapper;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LMSCourse.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _repository;
        private readonly IMapper _mapper;

        public SettingsService(ISettingsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PasswordPolicy?> GetPasswordPolicy()
        {
            var pwdPolicy = await _repository.GetPolicyAsync();
            if (pwdPolicy == null) return null;

            return pwdPolicy;
        }

        public async Task<PasswordPolicy> UpdatePasswordPolicy(PasswordPolicy pwdPolicyDto)
        {
            var pwdPolicy = await _repository.GetPolicyAsync();

            if (pwdPolicy == null) return null;

            _mapper.Map(pwdPolicyDto, pwdPolicy);

            await _repository.UpdatePolicyAsync(pwdPolicy);

            return pwdPolicy;
        }

        public async Task<(bool IsValid, List<string> Errors)> ValidateAsync(string password)
        {
            var policy = await _repository.GetPolicyAsync();
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be empty.");
                return (false, errors);
            }

            if (password.Length < policy.MinLength)
                errors.Add($"Mật khẩu phải có ít nhất {policy.MinLength} ký tự.");

            if (policy.RequireDigit && !password.Any(char.IsDigit))
                errors.Add("Mật khẩu phải có ít nhất một số.");

            if (policy.RequireLowercase && !password.Any(char.IsLower))
                errors.Add("Mật khẩu phải có chữ thường.");

            if (policy.RequireUppercase && !password.Any(char.IsUpper))
                errors.Add("Mật khẩu phải có chữ hoa.");

            if (policy.RequireNonAlphanumeric && !Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                errors.Add("Mật khẩu phải có ký tự đặc biệt.");

            if (policy.RequiredUniqueChars > 1)
            {
                int uniqueCount = password.Distinct().Count();
                if (uniqueCount < policy.RequiredUniqueChars)
                    errors.Add($"Mật khẩu phải có {policy.RequiredUniqueChars} ký tự duy nhất.");
            }

            return (!errors.Any(), errors);
        }
    }
}

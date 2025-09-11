using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMSCourse.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public TokenService(IConfiguration config, IUserService userService, IUserRepository userRepository)
        {
            _config = config;
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("type", "access")
            };

            var roles = await _userService.GetRolesNameByIdAsync(user.UserId);

            //Role Permissions
            var permissions = await _userService.GetPermissionsNameByIdAsync(user.UserId);

            //User Permissions
            permissions.AddRange(await _userService.GetUserPermissionsNameById(user.UserId));

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("type", "refresh")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            try
            {
                Console.WriteLine("=== Refresh Debug ===");
                Console.WriteLine($"Token: {refreshToken}");

                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                Console.WriteLine("Claims:");
                foreach (var c in principal.Claims)
                {
                    Console.WriteLine($"  {c.Type}: {c.Value}");
                }

                if (principal.FindFirst("type")?.Value != "refresh")
                    throw new SecurityTokenException("Invalid refresh token type");

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new SecurityTokenException("Invalid refresh token payload");

                var user = await _userRepository.GetByIdAsync(int.Parse(userId));
                if (user == null)
                    throw new SecurityTokenException("User not found");

                var newAccessToken = await GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken(user);

                return (newAccessToken, newRefreshToken);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid refresh token", ex);
            }
        }

        public string GenerateResetPasswordToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("type", "reset")
                }),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateResetPasswordToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true
            }, out _);

            // Chỉ chấp nhận token loại "reset"
            var typeClaim = principal.FindFirst("type")?.Value;
            if (typeClaim != "reset")
                throw new SecurityTokenException("Invalid token type");

            return principal;
        }
    }
}

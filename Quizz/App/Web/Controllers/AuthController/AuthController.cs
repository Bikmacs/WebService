using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Quizz.App.Domain.Models;
using Quizz.App.Domain.Models.Services;
using Quizz.App.Domain.Models.Services.AuthService;
using Quizz.App.Domain.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quizz.App.Web.Controllers.AuthController
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            if (string.IsNullOrEmpty(authRequest.Username) || string.IsNullOrEmpty(authRequest.Password))
                return Unauthorized("Неверное имя пользователя или пароль.");

            var user = await _authService.Authenticate(authRequest);
            if (user == null)
                return Unauthorized("Неверное имя пользователя или пароль.");

            // 🔥 Генерация JWT токена
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Токен истекает через 1 час
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
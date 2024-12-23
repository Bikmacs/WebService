using Microsoft.AspNetCore.Mvc;
using Quizz.App.Domain.Models.User;
using Quizz.Services;

namespace Quizz.App.Web.Controllers.AuthController;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User login)
    {
        var user = await _authService.Authenticate(login.Username, login.Password);
        if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            return Unauthorized("Неверное имя пользователя или пароль.");

        if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            return Unauthorized("Неверное имя пользователя или пароль.");
        return Ok("Вход выполнен успешно.");
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        var user = await _authService.Register(newUser);

        if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
            return BadRequest("Username and/or password are required");

        if (!user) return Unauthorized("Пользователь с таким именем уже существует");
        return Ok("Регистрация прошла успешно.");
    }
}
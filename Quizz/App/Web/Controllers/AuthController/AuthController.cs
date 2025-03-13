using Microsoft.AspNetCore.Mvc;
using Quizz.App.Domain.Models;
using Quizz.App.Domain.Models.Services;
using Quizz.App.Domain.Models.User;

namespace Quizz.App.Web.Controllers.AuthController;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
    {
        if (string.IsNullOrEmpty(authRequest.Username) || string.IsNullOrEmpty(authRequest.Password))
            return Unauthorized("Неверное имя пользователя или пароль.");

        var user = await authService.Authenticate(authRequest);
        if (user == null)
            return Unauthorized("Неверное имя пользователя или пароль.");
        return Ok("Вход выполнен успешно.");
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        Random rnd = new Random();
        rnd.Next(1, 100);
        if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
            return BadRequest("Username and/or password are required");
        
        var user = await authService.Register(newUser);

        if (!user) return Unauthorized("Пользователь с таким именем уже существует");
        return Ok($"{rnd.ToString()}Регистрация прошла успешно.");
        
        
       
    }
}
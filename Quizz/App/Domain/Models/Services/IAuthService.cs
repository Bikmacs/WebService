namespace Quizz.App.Domain.Models.Services;

public interface IAuthService
{
    Task<User.User> Authenticate(AuthRequest request);
    Task<bool> Register(User.User newUser);
}
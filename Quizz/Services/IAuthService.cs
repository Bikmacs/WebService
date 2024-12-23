using Quizz.App.Domain.Models.User;

namespace Quizz.Services;

public interface IAuthService
{
    Task<User> Authenticate(string username, string password);
    Task<bool> Register(User newUser);
}
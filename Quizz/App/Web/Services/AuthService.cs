//Quizz.App.Web/Services/AuthService.cs

using Microsoft.EntityFrameworkCore;
using Quizz.App.Domain.Models.User;
using Quizz.App.Infrastructure.Context;

namespace Quizz.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationContext _context;
    private IAuthService _authServiceImplementation;

    public AuthService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User> Authenticate(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;

        return user;
    }

    public async Task<bool> Register(User newUser)
    {
        if (newUser == null) return false;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
        if (user != null) return false;

        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return true;
    }
}
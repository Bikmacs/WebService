//Quizz.App.Web/Services/AuthService.cs

using Microsoft.EntityFrameworkCore;
using Quizz.App.Infrastructure.Context;

namespace Quizz.App.Domain.Models.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ApplicationContext _context;
    public AuthService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User.User> Authenticate(AuthRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) return null;

        return user;
    }
    public async Task<bool> Register(User.User newUser)
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
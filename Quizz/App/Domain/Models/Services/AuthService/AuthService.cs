//Quizz.App.Web/Services/AuthService.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quizz.App.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Quizz.App.Domain.Models.Services.AuthService;

public class AuthService(ApplicationContext context, IConfiguration configuration) : IAuthService
{
    private readonly ApplicationContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    
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
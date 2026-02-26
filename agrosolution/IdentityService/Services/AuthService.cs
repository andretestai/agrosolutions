using IdentityService.DTOs;
using IdentityService.Models;

namespace IdentityService.Services;

public class AuthService
{
    private static readonly List<User> _users = new();

    public AuthResponse? Register(RegisterRequest req)
    {
        if (_users.Any(u => u.Email == req.Email))
            return null;

        var user = new User
        {
            Name = req.Name,
            Email = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        _users.Add(user);

        return new AuthResponse(
            Token: GenerateToken(user),
            UserId: user.Id.ToString(),
            Name: user.Name
        );
    }

    public AuthResponse? Login(LoginRequest req)
    {
        var user = _users.FirstOrDefault(u => u.Email == req.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return null;

        return new AuthResponse(
            Token: GenerateToken(user),
            UserId: user.Id.ToString(),
            Name: user.Name
        );
    }

    private static string GenerateToken(User user)
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
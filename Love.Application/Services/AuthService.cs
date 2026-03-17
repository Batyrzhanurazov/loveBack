using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Love.Application.Interfaces;
using Love.Domain.Common;
using Love.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Love.Application.Services;

public class AuthService(IRepository repository) : IAuthService
{
    public async Task<Either<Exception, LoginResult>> LoginAsync(string login, string password)
    {
        var userResult = await repository.GetUserAsync(login);
        if (userResult.IsLeft)
            return userResult.Left;
        if (userResult.Right == null)
            return new LoginResult { IsLogged = false };

        var hashedPassword = HashPassword(password);
        var isLogged = userResult.Right.Password == hashedPassword;

        if (!isLogged)
            return new LoginResult { IsLogged = false };

        var token = GenerateToken(login, userResult.Right.Role.ToString()!);

        return new LoginResult
        {
            IsLogged = true,
            Role = userResult.Right.Role,
            Token = token
        };
    }

    private static string GenerateToken(string login, string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!));
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
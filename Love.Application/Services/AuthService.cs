using System.Security.Cryptography;
using System.Text;
using Love.Application.Interfaces;
using Love.Domain.Common;
using Love.Domain.Models;

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
        
        return new LoginResult
        {
            IsLogged = isLogged,
            Role = userResult.Right.Role,
        };
    }

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
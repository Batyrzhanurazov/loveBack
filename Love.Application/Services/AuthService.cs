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
            return new LoginResult
            {
                IsLogged = false
            };
        
        var isLogged = Isopoh.Cryptography.Argon2.Argon2.Verify(userResult.Right.Password, password);
        return new LoginResult
        {
            IsLogged = isLogged,
            Role = userResult.Right.Role,
        };
    }
}
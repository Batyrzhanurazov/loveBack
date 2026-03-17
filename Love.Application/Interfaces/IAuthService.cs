using Love.Domain.Common;
using Love.Domain.Models;

namespace Love.Application.Interfaces;

public interface IAuthService
{ 
    Task<Either<Exception, LoginResult>> LoginAsync(string login, string password);
}
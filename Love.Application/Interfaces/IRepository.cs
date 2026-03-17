using Love.Domain.Common;
using Love.Domain.Models;

namespace Love.Application.Interfaces;

public interface IRepository
{
    Task<Either<Exception, bool>> HasUsersAsync();
    Task<Either<Exception, User?>> GetUserAsync(string login);
    Task<Either<Exception, int>> AddLoveMessageAsync(string message);
    Task<Either<Exception, int>> AddUsersAsync(IEnumerable<User> users);
    Task<Either<Exception, LoveMessage>> GetLoveMessageAsync();
    Task<Either<Exception, bool>> HasMessagesAsync();
    Task<Either<Exception, int>> AddMessagesAsync(IEnumerable<LoveMessage> messages);
}
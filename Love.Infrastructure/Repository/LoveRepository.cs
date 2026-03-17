using System.Data;
using System.Globalization;
using Dapper;
using Love.Application.Interfaces;
using Love.Domain.Common;
using Love.Domain.Models;

namespace Love.Infrastructure.Repository;

public class LoveRepository(IDbConnection connection) : IRepository
{
    public async Task<Either<Exception, User?>> GetUserAsync(string login)
    {
        try
        {
            var sql = "SELECT login, password, role FROM users WHERE login = @login";
            var result = await connection.QueryFirstOrDefaultAsync<User?>(sql, new { login });
            Console.WriteLine(result.Role);
            return result;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Either<Exception, int>> AddLoveMessageAsync(string message)
    {
        try
        {
            var sql = "INSERT INTO messages (message, is_used) VALUES (@message, FALSE) RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, new { message });
            return id;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public async Task<Either<Exception, bool>> HasUsersAsync()
    {
        try
        {
            var sql = "SELECT COUNT(1) FROM users";
            var count = await connection.ExecuteScalarAsync<int>(sql);
            return count > 0;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Either<Exception, int>> AddUsersAsync(IEnumerable<User> users)
    {
        try
        {
            var sql = "INSERT INTO users (login, password, role) VALUES (@Login, @Password, @Role)";
            return await connection.ExecuteAsync(sql, users);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Either<Exception, LoveMessage>> GetLoveMessageAsync()
    {
        try
        {
            var message = await TryGetUnusedMessageAsync();
        
            if (message is null)
            {
                await ResetAllMessagesAsync();
                message = await TryGetUnusedMessageAsync();
            }

            return message!;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Either<Exception, bool>> HasMessagesAsync()
    {
        try
        {
            var sql = "SELECT COUNT(1) FROM messages";
            var count = await connection.ExecuteScalarAsync<int>(sql);
            return count > 0;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public async Task<Either<Exception, int>> AddMessagesAsync(IEnumerable<LoveMessage> messages)
    {
        try
        {
            var sql = "INSERT INTO messages (is_used, message) VALUES (@IsUsed, @Message)";
            return await connection.ExecuteAsync(sql, messages);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    private async Task<LoveMessage?> TryGetUnusedMessageAsync()
    {
        var sql = "SELECT id, message, is_used FROM messages WHERE is_used = FALSE ORDER BY RANDOM() LIMIT 1";
        return await connection.QueryFirstOrDefaultAsync<LoveMessage?>(sql);
    }

    private async Task ResetAllMessagesAsync()
    {
        var sql = "UPDATE messages SET is_used = FALSE";
        await connection.ExecuteAsync(sql);
    }
}
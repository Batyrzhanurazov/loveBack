namespace Love.Application.Interfaces;

public interface ILoveService
{
    Task<bool> AddMessageAsync(string message);
    Task<string> GetMessageAsync();
    Task PingDbAsync();
}
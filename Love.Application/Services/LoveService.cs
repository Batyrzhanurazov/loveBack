using Love.Application.Interfaces;

namespace Love.Application.Services;

public class LoveService(IRepository repository) : ILoveService
{
    public async Task<bool> AddMessageAsync(string message)
    {
        var result = await repository.AddLoveMessageAsync(message);
        return !result.IsLeft;
    }

    public async Task<string> GetMessageAsync()
    {
        var messageResult = await repository.GetLoveMessageAsync();
        if (messageResult.IsLeft)
            return "Я тебя люблю, но скажи мужу(мне) что с БД что-то не так";

        return messageResult.Right.Message;
    }
}
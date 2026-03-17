using Love.Application.Interfaces;
using Love.Domain.Enums;
using Love.Domain.Models;

namespace Love.Infrastructure.Initializer;

public class DatabaseSeeder(IRepository repository)
{
    public async Task SeedAsync()
    {
        await SeedUsers();
        await SeedMessages();
    }

    private async Task SeedUsers()
    {
        var hasUserResult = await repository.HasUsersAsync();
        if (hasUserResult.IsLeft)
        {
            return;
        }
        if (hasUserResult.Right)
            return;

        var users = new[]
        {
            new User
            {
                Login = Environment.GetEnvironmentVariable("AMIRA_LOGIN")!,
                Password = Isopoh.Cryptography.Argon2.Argon2.Hash(Environment.GetEnvironmentVariable("AMIRA_PASSWORD")!),
                Role = Role.User
            },
            new User
            {
                Login = Environment.GetEnvironmentVariable("ADMIN_LOGIN")!,
                Password = Isopoh.Cryptography.Argon2.Argon2.Hash(Environment.GetEnvironmentVariable("ADMIN_PASSWORD")!),
                Role = Role.Admin
            }
        };

        await repository.AddUsersAsync(users);
    }

    private async Task SeedMessages()
    {
        var hasMessages = await repository.HasMessagesAsync();
        if (hasMessages.IsLeft)
            return;

        if (hasMessages.Right)
            return;

        var messages = new[]
        {
            new LoveMessage
            {
                IsUsed = false,
                Message = "Для меня ты совершенство. И моё сердце будет любить тебя, пока ты не станешь такой. (Тут должно быть фото мумии.)"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Мне нравится, когда ты плачешь потом смеешься. Мне нравится когда ты опаздываешь и нервничаешь, мне нравится твоя самая прекрасная копна волос, мне нравится что перед сном мы можем говорить."
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "У тебя самая лучшая жопка, причина нашей первой поездки в машине"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Я хочу провести короткую жизнь с тобой, чем проведу бессмертную в одиночестве"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Я люблю тебя, прости если не так часто это говорю. Но я люблю тебя всегда."
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Хочу смотреть как ты варишь кукурузу до конца моих дней!"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Когда купим билеты в кругосветку? Давай договоримся, на определенную дату. Как тебе например через 10 лет? Вытерпишь меня на 130 дней?"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Я никогда в жизни не был так счастлив, как возле тебяю. Я именно там где я должен быть"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "То что есть между нами так хрупко, но в то же время так целостно. Я всегда найду дорогу к тебе!"
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Клянусь страстно любить тебя теперь и навсегда. Я обещаю сохранить это чувство и знаю: эта любовь одна и на всю жизнь."
            },
            new LoveMessage
            {
                IsUsed = false,
                Message = "Только мне одному, быть может, известно, что ты лучшая женщина на свете. Похоже, только я один понимаю, как замечательно всё, что ты делаешь. Как восхитительно ты всё воспринимаешь, как разговариваешь с людьми и как почти во всём проявляются твои лучшие качества: твои доброта и честность, если бы это было не правдой я бы с кем нибудь дрался до сих пор!"
            }
        };

        await repository.AddMessagesAsync(messages);
    }
}
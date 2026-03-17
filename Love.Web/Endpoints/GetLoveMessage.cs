using FastEndpoints;
using Love.Application.Interfaces;

namespace Love.Web.Endpoints;

public class GetLoveMessage(ILoveService service) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/loveMessage");
        Roles("User");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var message = await service.GetMessageAsync();
        await SendOkAsync(message, ct);
    }
}
using FastEndpoints;
using JetBrains.Annotations;
using Love.Application.Interfaces;

namespace Love.Web.Endpoints;

public class AddLoveMessage(ILoveService loveService) : Endpoint<AddLoveMessageRequest>
{
    public override void Configure()
    {
        Post("loveMessage");
        Roles("Admin");
    }

    public override async Task HandleAsync(AddLoveMessageRequest req, CancellationToken ct)
    {
        var result = await loveService.AddMessageAsync(req.Message);
        
        if (result)
            await SendOkAsync(ct);
        else
            ThrowError("Failed to add message");
    }
}

[UsedImplicitly]
public class AddLoveMessageRequest
{
    public required string Message { get; init; }
}
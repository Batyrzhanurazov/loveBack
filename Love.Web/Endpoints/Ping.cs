using FastEndpoints;
using Love.Application.Interfaces;

namespace Love.Web.Endpoints;

public class Ping(ILoveService service) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/ping");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await service.PingDbAsync();
        await SendOkAsync(ct);
    }
}
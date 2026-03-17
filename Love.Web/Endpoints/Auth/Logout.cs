using FastEndpoints;
using Microsoft.AspNetCore.Authentication;

namespace Love.Web.Endpoints.Auth;

public class Logout : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/logout");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await HttpContext.SignOutAsync("cookie");
        
        await SendNoContentAsync(ct);
    }
}
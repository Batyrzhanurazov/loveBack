using System.Security.Claims;
using FastEndpoints;

namespace Love.Web.Endpoints.Auth;

public class Me : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/me");
        AuthSchemes("cookie");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var login = User.FindFirst(ClaimTypes.Name)?.Value;
        Console.WriteLine(login + " " + role);
        await SendOkAsync(new { login, role }, ct);
    }
}
using System.Security.Claims;
using FastEndpoints;
using JetBrains.Annotations;
using Love.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Love.Web.Endpoints.Auth;

public class Login(IAuthService authService) : Endpoint<LoginRequest>
{
    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var loginResult = await authService.LoginAsync(req.Login, req.Password);

        if (loginResult.IsLeft)
        {
            await SendErrorsAsync(500, ct);
            return;
        }

        if (!loginResult.Right.IsLogged)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await SendOkAsync(new { token = loginResult.Right.Token, role = loginResult.Right.Role.ToString() }, ct);
    }
}

[UsedImplicitly]
public record LoginRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; } 
}
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
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, req.Login),
            new(ClaimTypes.Role, loginResult.Right.Role.ToString()!)
        };

        Console.WriteLine(loginResult.Right.Role.ToString()!);
        var identity = new ClaimsIdentity(claims, "cookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("cookie", principal);
        await SendOkAsync(ct);
    }
}

[UsedImplicitly]
public record LoginRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; } 
}
using System.Text;
using DotNetEnv;
using FastEndpoints;
using Love.Application.Interfaces;
using Love.Application.Services;
using Love.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
var frontendUrl = builder.Configuration["FrontendConnection:Default"];

builder.Services.AddFastEndpoints();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILoveService, LoveService>();
builder.Services.AddAuthorization();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("Default")!
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .WithOrigins(frontendUrl!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api/v1";
});

app.Run();
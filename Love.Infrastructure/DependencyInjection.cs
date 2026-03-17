using System.Data;
using Love.Application.Interfaces;
using Love.Infrastructure.Initializer;
using Love.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Love.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        services.AddScoped<IRepository, LoveRepository>();
        services.AddScoped<DatabaseInitializer>();
        services.AddScoped<DatabaseSeeder>();
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        
        await scope.ServiceProvider
            .GetRequiredService<DatabaseInitializer>()
            .InitializeAsync();
        
        await scope.ServiceProvider
            .GetRequiredService<DatabaseSeeder>()
            .SeedAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Starter.Application.Interfaces;

namespace Starter.Infrastructure.Database;
public static class DependencyInjection
{
    public static void AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<StarterContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("StarterDatabase")));

        builder.Services.AddScoped<StarterContextInitializer>();
        builder.Services.AddScoped<IStarterContext, StarterContext>();
    }

    public static async Task UseDatabaseAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        using var scope = host.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<StarterContextInitializer>();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<StarterContextInitializer>>();

        logger.LogInformation("Initializing database");
        await dbInitializer.InitializeAsync(cancellationToken);
    }
}

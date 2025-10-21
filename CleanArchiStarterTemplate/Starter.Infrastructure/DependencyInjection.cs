using Microsoft.Extensions.Hosting;
using Starter.Infrastructure.Database;
using Starter.Infrastructure.Tracing;

namespace Starter.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddDatabase();
        builder.AddTracing();
        //builder.AddFactories();
        //builder.AddProviders();
    }

    public static async Task UseInfrastructureAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if(env != "NSwag")
        {
            // UNCOMMENT the following line to use the database
            // await host.UseDatabaseAsync(cancellationToken);
        }
    }
}
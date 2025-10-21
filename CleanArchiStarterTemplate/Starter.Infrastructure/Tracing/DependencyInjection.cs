using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Starter.Application.Interfaces;

namespace Starter.Infrastructure.Tracing;
public static class DependencyInjection
{
    public static void AddTracing(this IHostApplicationBuilder builder)
    {
        // Register the tracing service
        builder.Services.AddSingleton<ITracingService, TracingService>();
    }

    //public static void UseTracing(this IHost host)
    //{
    //    // This method can be used to configure any additional tracing settings if needed
    //}
}

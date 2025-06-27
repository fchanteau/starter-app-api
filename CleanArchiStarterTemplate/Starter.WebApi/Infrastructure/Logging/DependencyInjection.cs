using Serilog;
using Serilog.Enrichers.OpenTelemetry;
using Serilog.Sinks.Grafana.Loki;

namespace Starter.WebApi.Infrastructure.Logging;

public static class DependencyInjection
{
    public static void AddLogging(this IHostApplicationBuilder builder)
    {
        var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithOpenTelemetryTraceId()
                .Enrich.WithOpenTelemetrySpanId()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
                    "| TraceId: {TraceId} | SpanId: {SpanId} " +
                    "{NewLine}{Exception}");

        // Ajout du sink Loki uniquement en production
        if(builder.Environment.IsProduction())
        {
            loggerConfig = loggerConfig.WriteTo.GrafanaLoki(
                uri: builder.Configuration["GrafanaCloud:Loki:Url"]!,
                labels: new List<LokiLabel>
                {
                new() { Key = "app", Value = "StarterApp" },
                new() { Key = "environment", Value = builder.Environment.EnvironmentName },
                new() { Key = "version", Value = "1.0.0" },
                new() { Key = "instance", Value = Environment.MachineName }
                },
                credentials: new LokiCredentials
                {
                    Login = builder.Configuration["GrafanaCloud:Loki:UserId"]!,
                    Password = builder.Configuration["GrafanaCloud:ServiceAccount:Token"]!,
                },
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                batchPostingLimit: 10,
                period: TimeSpan.FromSeconds(2)
            );
        }

        Log.Logger = loggerConfig.CreateLogger();
    }

    public static void UseLogging(this WebApplication app)
    {
        // Configure Serilog request logging
        app.UseSerilogRequestLogging();
    }
}

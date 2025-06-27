using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Starter.WebApi.Infrastructure.Otel;

public static class DependencyInjection
{
    public static void AddOtel(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("StarterApp"))
                    .AddSource("StarterApp")
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.Filter = (context) =>
                        {
                            // Filtrer les health checks par exemple
                            return !context.Request.Path.StartsWithSegments("/health");
                        };
                    })
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddConsoleExporter();

                // Ajout de l'exporter OTLP uniquement en production
                if(builder.Environment.IsProduction())
                {
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(builder.Configuration["GrafanaCloud:Tempo:Endpoint"]!);

                        var tempoCredentials = Convert.ToBase64String(
                            System.Text.Encoding.UTF8.GetBytes(
                                $"{builder.Configuration["GrafanaCloud:Tempo:UserId"]}:" +
                                $"{builder.Configuration["GrafanaCloud:ServiceAccount:Token"]}"
                            )
                        );
                        options.Headers = $"Authorization=Basic {tempoCredentials}";
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
                }
            });
    }

    public static void UseOtel(this WebApplication app)
    {
        // Si vous avez besoin de configurer des middlewares spécifiques pour OpenTelemetry, faites-le ici
        // Par exemple, vous pouvez ajouter des middlewares pour exporter les traces ou les métriques

        app.Use(async (context, next) =>
        {
            using var activity = Activity.Current;
            if(activity != null)
            {
                activity.SetTag("user.id", context.User?.Identity?.Name ?? "anonymous");
                activity.SetTag("http.request_id", context.TraceIdentifier);
            }
            await next();
        });
    }
}

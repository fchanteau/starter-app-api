// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
                            // Filter out path beginning with /healthz or /swagger
                            return !context.Request.Path.StartsWithSegments("/healthz") ||
                                   !context.Request.Path.StartsWithSegments("/swagger");
                        };
                    })
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddConsoleExporter()
                    // On envoie les traces vers ApplicationInsights (PROD only mais la pour tester)
                    .AddAzureMonitorTraceExporter(options =>
                    {
                        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddAzureMonitorMetricExporter(options =>
                {
                    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
                });
            });
    }

    public static void UseOtel(this WebApplication app)
    {
        // Si vous avez besoin de configurer des middlewares spécifiques pour OpenTelemetry, faites-le ici
        // Par exemple, vous pouvez ajouter des middlewares pour exporter les traces ou les métriques

        app.Use(async (context, next) =>
        {
            var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();

            using var activity = Activity.Current;
            if (activity != null)
            {
                activity.SetTag("user.id", context.User?.Identity?.Name ?? "anonymous");
                activity.SetTag("http.request_id", context.TraceIdentifier);
                activity.SetTag("correlation_id", correlationId ?? "none");
            }
            await next();
        });
    }
}

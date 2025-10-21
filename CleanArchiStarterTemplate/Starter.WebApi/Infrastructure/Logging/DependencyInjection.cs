// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Serilog;
using Serilog.Enrichers.OpenTelemetry;

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

        // Envoi des logs vers ApplicationInsights
        loggerConfig.WriteTo.ApplicationInsights(builder.Configuration["ApplicationInsights:ConnectionString"], TelemetryConverter.Traces);

        Log.Logger = loggerConfig.CreateLogger();
    }

    public static void UseLogging(this WebApplication app)
    {
        // Configure Serilog request logging
        app.UseSerilogRequestLogging();
    }
}

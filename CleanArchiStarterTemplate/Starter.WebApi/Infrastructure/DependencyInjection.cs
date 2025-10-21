// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Starter.Infrastructure.Database;
using Starter.WebApi.Infrastructure.Logging;
using Starter.WebApi.Infrastructure.OpenApi;
using Starter.WebApi.Infrastructure.Otel;

namespace Starter.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static void AddWebInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddProblemDetails();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<StarterContext>(name: "DatabaseHealthcheck");

        // project specific services
        // example : builder.AddProjectAuthentication();
        builder.AddOpenApi();
        builder.AddOtel();
        builder.AddLogging();
    }

    public static void UseWebInfrastructure(this WebApplication app)
    {
        // Common MVC configuration
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // project specific middleware
        // example : app.UseProjectCors();
        app.UseWebOpenApi();
        app.UseLogging();
        app.UseOtel();

        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            ResponseWriter = WriteResponse
        });
    }

    private static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("status",
                    healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("description",
                    healthReportEntry.Value.Description);
                jsonWriter.WriteStartObject("data");

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    JsonSerializer.Serialize(jsonWriter, item.Value,
                        item.Value?.GetType() ?? typeof(object));
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        return context.Response.WriteAsync(
            Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}

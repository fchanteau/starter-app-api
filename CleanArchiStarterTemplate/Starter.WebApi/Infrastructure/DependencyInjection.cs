using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Starter.Infrastructure.Database;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        builder.Services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "v1";
            config.Title = "Starter API";
            config.Version = "v1";
        });

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<StarterContext>(name: "DatabaseHealthcheck");

        // project specific services
        // example : builder.AddProjectAuthentication();
    }

    public static void UseWebInfrastructure(this WebApplication app)
    {
        // project specific middleware
        // example : app.UseProjectCors();

        // Common MVC configuration
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseOpenApi();
        app.UseSwaggerUi();

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
        using(var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach(var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("status",
                    healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("description",
                    healthReportEntry.Value.Description);
                jsonWriter.WriteStartObject("data");

                foreach(var item in healthReportEntry.Value.Data)
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

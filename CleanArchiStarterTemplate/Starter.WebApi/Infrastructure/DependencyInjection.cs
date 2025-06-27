using Microsoft.AspNetCore.Mvc;
using Starter.Infrastructure.Database;
using Starter.WebApi.Infrastructure.Otel;
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
        builder.AddOtel();
        builder.AddLogging();
    }

    public static void UseWebInfrastructure(this WebApplication app)
    {
        // Common MVC configuration
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseOpenApi();
        app.UseSwaggerUi();
    }
}

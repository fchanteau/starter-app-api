using Microsoft.AspNetCore.Mvc;
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
    }
}

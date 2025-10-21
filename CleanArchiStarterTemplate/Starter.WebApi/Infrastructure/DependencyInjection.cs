// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.WebApi.Infrastructure.Logging;
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
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Starter.WebApi.Infrastructure.OpenApi;

public static class DependencyInjection
{
    public static void AddOpenApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "v1";
            config.Title = "Starter API";
            config.Version = "v1";
            config.OperationProcessors.Add(new NSwag.Generation.Processors.OperationProcessor(context =>
            {
                context.OperationDescription.Operation.Parameters.Add(new NSwag.OpenApiParameter
                {
                    Name = "X-Correlation-Id",
                    Kind = NSwag.OpenApiParameterKind.Header,
                    Type = NJsonSchema.JsonObjectType.String,
                    IsRequired = false,
                    Description = "Correlation Id"
                });
                return true;
            }));
        });
    }

    public static void UseWebOpenApi(this WebApplication app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi();
    }
}

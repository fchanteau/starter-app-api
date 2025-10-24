// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace Starter.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddMediatR(options =>
        //{
        //    options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        //});

        services.AddMediator();

        return services;
    }
}

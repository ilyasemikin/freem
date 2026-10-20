﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityIdNameConverters(this IServiceCollection services)
    {
        services.TryAddSingleton(EntityIdFactory.Instance);
        services.TryAddSingleton(EntityIdNameProvider.Instance);
        
        return services;
    }
}
﻿using Freem.Entities.Events.Producer.Abstractions;
using Freem.Entities.Events.Producer.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Events.Producer.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventProduction<TEventPublisher>(
        this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TEventPublisher : IEventPublisher
    {
        var descriptor = new ServiceDescriptor(typeof(IEventPublisher), typeof(TEventPublisher), lifetime);
        
        services.TryAdd(descriptor);
        services.TryAddTransient<EventProducer>();
        
        return services;
    }

    public static IServiceCollection AddEventProduction<TEventPublisher>(
        this IServiceCollection services, 
        Func<IServiceProvider, TEventPublisher> factory, 
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TEventPublisher : IEventPublisher
    {
        var descriptor = new ServiceDescriptor(typeof(IEventPublisher), provider => factory(provider), lifetime);
        
        services.TryAdd(descriptor);
        services.TryAddTransient<EventProducer>();

        return services;
    }
}
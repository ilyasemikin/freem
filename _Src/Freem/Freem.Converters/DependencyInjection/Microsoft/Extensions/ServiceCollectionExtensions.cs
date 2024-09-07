using Freem.Converters.Collections;
using Freem.Converters.Collections.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Converters.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConvertersCollection<TInput, TOutput>(
        this IServiceCollection services,
        Action<IServiceProvider, ConvertersCollectionBuilder<TInput, TOutput>> builderAction,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TInput : class
        where TOutput : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builderAction);
        
        var descriptor = new ServiceDescriptor(
            typeof(ConvertersCollection<TInput, TOutput>),
            provider => CreateConvertersCollection(provider, builderAction), 
            lifetime);
        
        services.TryAdd(descriptor);

        return services;
    }
    
    public static IServiceCollection AddConvertersCollection<TInput, TOutput>(
        this IServiceCollection services,
        Action<ConvertersCollectionBuilder<TInput, TOutput>> builderAction,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TInput : class
        where TOutput : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builderAction);
        
        var descriptor = new ServiceDescriptor(
            typeof(ConvertersCollection<TInput, TOutput>),
            _ => CreateConvertersCollection(builderAction), 
            lifetime);
        
        services.TryAdd(descriptor);

        return services;
    }

    private static ConvertersCollection<TInput, TOutput> CreateConvertersCollection<TInput, TOutput>(
        IServiceProvider services,
        Action<IServiceProvider, ConvertersCollectionBuilder<TInput, TOutput>> builderAction)
        where TInput : class
        where TOutput : class
    {
        var builder = new ConvertersCollectionBuilder<TInput, TOutput>();
        builderAction(services, builder);
        return builder.Build();
    }
    
    private static ConvertersCollection<TInput, TOutput> CreateConvertersCollection<TInput, TOutput>(
        Action<ConvertersCollectionBuilder<TInput, TOutput>> builderAction)
        where TInput : class
        where TOutput : class
    {
        var builder = new ConvertersCollectionBuilder<TInput, TOutput>();
        builderAction(builder);
        return builder.Build();
    }
}
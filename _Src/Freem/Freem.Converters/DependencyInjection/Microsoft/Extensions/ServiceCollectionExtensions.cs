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
    
    public static IServiceCollection AddConvertersCollection<TInput1, TInput2, TOutput>(
        this IServiceCollection services,
        Action<IServiceProvider, ConvertersCollectionBuilder<TInput1, TInput2, TOutput>> builderAction,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TInput1 : class
        where TInput2 : class
        where TOutput : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builderAction);
        
        var descriptor = new ServiceDescriptor(
            typeof(ConvertersCollection<TInput1, TInput2, TOutput>),
            provider => CreateConvertersCollection(provider, builderAction), 
            lifetime);
        
        services.TryAdd(descriptor);

        return services;
    }
    
    public static IServiceCollection AddConvertersCollection<TInput1, TInput2, TOutput>(
        this IServiceCollection services,
        Action<ConvertersCollectionBuilder<TInput1, TInput2, TOutput>> builderAction,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TInput1 : class
        where TInput2 : class
        where TOutput : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builderAction);
        
        var descriptor = new ServiceDescriptor(
            typeof(ConvertersCollection<TInput1, TInput2, TOutput>),
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
    
    private static ConvertersCollection<TInput1, TInput2, TOutput> CreateConvertersCollection<TInput1, TInput2, TOutput>(
        IServiceProvider services,
        Action<IServiceProvider, ConvertersCollectionBuilder<TInput1, TInput2, TOutput>> builderAction)
        where TInput1 : class
        where TInput2 : class
        where TOutput : class
    {
        var builder = new ConvertersCollectionBuilder<TInput1, TInput2, TOutput>();
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
    
    private static ConvertersCollection<TInput1, TInput2, TOutput> CreateConvertersCollection<TInput1, TInput2, TOutput>(
        Action<ConvertersCollectionBuilder<TInput1, TInput2, TOutput>> builderAction)
        where TInput1 : class
        where TInput2 : class
        where TOutput : class
    {
        var builder = new ConvertersCollectionBuilder<TInput1, TInput2, TOutput>();
        builderAction(builder);
        return builder.Build();
    }
}
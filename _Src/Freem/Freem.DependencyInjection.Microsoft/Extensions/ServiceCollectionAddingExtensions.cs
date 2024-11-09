using Microsoft.Extensions.DependencyInjection;

namespace Freem.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionAddingExtensions
{
    #region TryAddServiceWithImplementedInterfaces

    public static bool TryAddSingletonServiceWithImplementedInterfaces<TImplementation>(
        this IServiceCollection services)
        where TImplementation : class
    {
        return services.TryAddServiceWithImplementedInterfaces(typeof(TImplementation), ServiceLifetime.Singleton);
    }
    
    public static bool TryAddScopedServiceWithImplementedInterface<TImplementation>(
        this IServiceCollection services)
        where TImplementation : class
    {
        return services.TryAddServiceWithImplementedInterfaces(typeof(TImplementation), ServiceLifetime.Scoped);
    }
    
    public static bool TryAddTransientServiceWithImplementedInterfaces<TImplementation>(
        this IServiceCollection services)
        where TImplementation : class
    {
        return services.TryAddServiceWithImplementedInterfaces(typeof(TImplementation), ServiceLifetime.Transient);
    }
    
    public static bool TryAddServiceWithImplementedInterfaces<TImplementation>(
        this IServiceCollection services, ServiceLifetime lifetime)
        where TImplementation : class
    {
        return services.TryAddServiceWithImplementedInterfaces(typeof(TImplementation), lifetime);
    }
    
    public static bool TryAddServiceWithImplementedInterfaces(
        this IServiceCollection services, Type implementationType, ServiceLifetime lifetime)
    {
        if (!implementationType.IsClass || implementationType.IsAbstract)
            throw new ArgumentException("must be not abstract class", nameof(implementationType));

        var count = services.Count(p => p.ServiceType == implementationType);
        if (count != 0)
            return false;
        
        var descriptor = new ServiceDescriptor(implementationType, implementationType, lifetime);
        services.Add(descriptor);
        
        var interfaces = implementationType.GetInterfaces();
        foreach (var @interface in interfaces)
        {
            if (@interface == typeof(IDisposable) || @interface == typeof(IAsyncDisposable))
                continue;

            descriptor = new ServiceDescriptor(
                @interface, 
                null, 
                (provider, _) => provider.GetRequiredService(implementationType),  
                lifetime);
            services.Add(descriptor);
        }

        return true;
    }

    #endregion

    #region TryAddExistedService

    public static bool TryAddSingletonExistedService<TService, TExisted>(this IServiceCollection services)
        where TExisted : TService
    {
        return services.TryAddExistedService<TService, TExisted>(ServiceLifetime.Singleton);
    }
    
    public static bool TryAddScopedExistedService<TService, TExisted>(this IServiceCollection services)
        where TExisted : TService
    {
        return services.TryAddExistedService<TService, TExisted>(ServiceLifetime.Scoped);
    }
    
    public static bool TryAddTransientExistedService<TService, TExisted>(this IServiceCollection services)
        where TExisted : TService
    {
        return services.TryAddExistedService<TService, TExisted>(ServiceLifetime.Transient);
    }
    
    public static bool TryAddExistedService<TService, TExisted>(
        this IServiceCollection services, 
        ServiceLifetime lifetime)
        where TExisted : TService
    {
        return services.TryAddExistedService(typeof(TService), typeof(TExisted), lifetime);
    }

    public static bool TryAddExistedService(
        this IServiceCollection services, 
        Type serviceType, Type existedType, ServiceLifetime lifetime)
    {
        if (!existedType.IsAssignableTo(serviceType))
            throw new ArgumentException($"must implements \"{serviceType.FullName}\"", nameof(existedType));
        
        var count = services.Count(service => service.ServiceType == existedType);
        if (count == 0)
            return false;
        
        count = services.Count(service => service.ServiceType == serviceType);
        if (count == 0)
            return false;

        var descriptor = new ServiceDescriptor(
            serviceType, 
            null, 
            (provider, _) => provider.GetRequiredService(existedType), 
            lifetime);
        services.Add(descriptor);
        
        return true;
    }

    #endregion

    #region AddExistedService

    public static IServiceCollection AddSingletonExistedService<TService, TExisted>(this IServiceCollection services)
        where TExisted : TService
    {
        return services.AddExistedService<TService, TExisted>(ServiceLifetime.Singleton);
    }
    
    public static IServiceCollection AddScopedExistedService<TService, TExisted>(this IServiceCollection services)
        where TExisted : TService
    {
        return services.AddExistedService<TService, TExisted>(ServiceLifetime.Scoped);
    }
    
    public static IServiceCollection AddTransientExistedService<TService, TExisted>(this IServiceCollection services)
        where TExisted : TService
    {
        return services.AddExistedService<TService, TExisted>(ServiceLifetime.Transient);
    }
    
    public static IServiceCollection AddExistedService<TService, TExisted>(
        this IServiceCollection services, 
        ServiceLifetime lifetime)
        where TExisted : TService
    {
        return services.AddExistedService(typeof(TService), typeof(TExisted), lifetime);
    }
    
    public static IServiceCollection AddExistedService(
        this IServiceCollection services, 
        Type serviceType, Type existedType, ServiceLifetime lifetime)
    {
        if (!existedType.IsAssignableTo(serviceType))
            throw new ArgumentException($"must implements \"{serviceType.FullName}\"", nameof(existedType));
        
        var count = services.Count(service => service.ServiceType == existedType);
        if (count == 0)
            throw new ArgumentException($"Service \"{existedType.FullName}\" does not exist", nameof(existedType));
        
        var descriptor = new ServiceDescriptor(
            serviceType, 
            null, 
            (provider, _) => provider.GetRequiredService(existedType), 
            lifetime);
        services.Add(descriptor);

        return services;
    }

    #endregion
}
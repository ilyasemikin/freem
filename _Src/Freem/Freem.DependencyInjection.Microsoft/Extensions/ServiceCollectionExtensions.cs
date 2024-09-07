using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceProvider BuildAndValidateServiceProvider(this IServiceCollection services)
    {
        var options = new ServiceProviderOptions { ValidateOnBuild = true };
        return services.BuildServiceProvider(options);
    }
    
    public static int CountByServiceType<TService>(this IServiceCollection services)
    {
        return services.Count(service => service.ServiceType == typeof(TService));
    }

    public static int CountByServiceType(this IServiceCollection services, Type serviceType)
    {
        return services.Count(service => service.ServiceType == serviceType);
    }

    public static bool ContainsServiceType<TService>(this IServiceCollection services)
    {
        return services.Any(service => service.ServiceType == typeof(TService));
    }

    public static bool TryGetDescriptor(
        this IServiceCollection services, Type serviceType, 
        [NotNullWhen(true)] out ServiceDescriptor? descriptor)
    {
        descriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        return descriptor is not null;
    }
    
    public static bool TryGetDescriptor<TService>(
        this IServiceCollection services, 
        [NotNullWhen(true)] out ServiceDescriptor? descriptor)
    {
        return services.TryGetDescriptor(typeof(TService), out descriptor);
    }
}
using Freem.Time.Abstractions;
using Freem.Time.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Time.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUtcCurrentTimeGetter(this IServiceCollection services)
    {
        services.TryAddSingleton<ICurrentTimeGetter, UtcCurrentTimeGetter>();
        
        return services;
    }
}
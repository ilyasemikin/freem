using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Serialization.Json.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntitiesJsonSerialization(this IServiceCollection services)
    {
        services.TryAddSingleton<EventJsonConverter>();
        
        return services;
    }
}
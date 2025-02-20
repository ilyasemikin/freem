using Freem.Entities.Events.Kafka.Implementations;
using Freem.Entities.Events.Production.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Freem.Entities.Events.Kafka.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaEventProduction(
        this IServiceCollection services, KafkaConfiguration configuration)
    {
        services.TryAddSingleton<EventTopicResolver>();

        return services.AddEventProduction(
            provider => new KafkaEventPublisher(
                configuration,
                provider.GetRequiredService<EventTopicResolver>(),
                provider.GetRequiredService<EventJsonConverter>(),
                provider.GetRequiredService<ILogger>()),
            ServiceLifetime.Singleton);
    }
}
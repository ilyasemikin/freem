using Freem.Entities.Events.Producer.DependencyInjection.Microsoft;
using Freem.Entities.Events.Producer.Kafka.Implementations;
using Freem.Entities.Events.Producer.Kafka.Models;
using Freem.Entities.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Freem.Entities.Events.Producer.Kafka.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaEventProduction(
        this IServiceCollection services, KafkaProducerConfiguration configuration)
    {
        services.TryAddSingleton<EventTopicResolver>();

        return services.AddEventProduction(
            provider => new KafkaEventPublisher(
                configuration,
                provider.GetRequiredService<EventTopicResolver>(),
                provider.GetRequiredService<EventJsonConverter>(),
                provider.GetRequiredService<ILogger<KafkaEventPublisher>>()),
            ServiceLifetime.Singleton);
    }
}
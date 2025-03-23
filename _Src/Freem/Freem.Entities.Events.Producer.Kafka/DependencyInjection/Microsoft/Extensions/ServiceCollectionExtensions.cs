using Freem.Entities.Events.Producer.DependencyInjection.Microsoft;
using Freem.Entities.Events.Producer.Kafka.Implementations;
using Freem.Entities.Events.Producer.Kafka.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Events.Producer.Kafka.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaEventProduction(
        this IServiceCollection services, KafkaProducerConfiguration configuration)
    {
        services.TryAddSingleton<EventTopicResolver>();

        return services.AddEventProduction(
            provider => ActivatorUtilities.CreateInstance<KafkaEventPublisher>(provider, configuration),
            ServiceLifetime.Singleton);
    }
}
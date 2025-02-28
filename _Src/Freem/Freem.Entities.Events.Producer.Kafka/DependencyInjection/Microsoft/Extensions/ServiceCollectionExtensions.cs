using Freem.Entities.Events.Producer.DependencyInjection.Microsoft;
using Freem.Entities.Events.Producer.Kafka.Implementations;
using Freem.Entities.Events.Producer.Kafka.Models;
using Freem.Entities.Serialization.Json;
using Freem.Entities.Serialization.Json.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Freem.Entities.Events.Producer.Kafka.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaEventProduction(
        this IServiceCollection services, Func<IServiceProvider, KafkaProducerConfiguration> configurationGetter)
    {
        services.TryAddSingleton<EventTopicResolver>();

        return services.AddEventProduction(
            provider =>
            {
                var configuration = configurationGetter(provider);
                return ActivatorUtilities.CreateInstance<KafkaEventPublisher>(provider, configuration);
            },
            ServiceLifetime.Singleton);
    }
}
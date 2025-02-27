using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Builders;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Implementations;
using Freem.Entities.Events.Consumer.Implementations;
using Freem.Entities.Events.Consumer.Kafka.Implementations;
using Freem.Entities.Events.Consumer.Kafka.Models;
using Freem.Entities.Serialization.Json;
using Freem.Entities.Serialization.Json.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Freem.Entities.Events.Consumer.Kafka.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaEventConsumption(
        this IServiceCollection services, 
        KafkaConsumerConfiguration configuration, Action<EventConsumersBuilder> builderAction)
    {
        var builder = new EventConsumersBuilder(services);
        builderAction(builder);

        var collection = builder.BuildTypesCollection();

        services.AddSingleton<IEventConsumerRunner>(provider => new EventConsumerRunner(provider));
        services.AddSingleton<KafkaEventConsumer>(provider =>
            new KafkaEventConsumer(
                configuration,
                provider.GetRequiredService<EventJsonConverter>(),
                new EventConsumersExecutor(collection, provider.GetRequiredService<IEventConsumerRunner>()),
                provider.GetRequiredService<ILogger<KafkaEventConsumer>>()));
        
        return services;
    }
}
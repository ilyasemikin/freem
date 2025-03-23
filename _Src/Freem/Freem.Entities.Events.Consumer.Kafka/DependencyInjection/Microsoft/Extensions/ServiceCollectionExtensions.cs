using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Builders;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Implementations;
using Freem.Entities.Events.Consumer.Implementations;
using Freem.Entities.Events.Consumer.Kafka.Implementations;
using Freem.Entities.Events.Consumer.Kafka.Models;
using Microsoft.Extensions.DependencyInjection;

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
        {
            var executor = ActivatorUtilities.CreateInstance<EventConsumersExecutor>(provider, collection);
            return ActivatorUtilities.CreateInstance<KafkaEventConsumer>(provider, executor);
        });
        
        return services;
    }
}
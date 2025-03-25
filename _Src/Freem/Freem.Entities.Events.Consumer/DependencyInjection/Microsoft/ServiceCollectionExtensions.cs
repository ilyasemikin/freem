using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Builders;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Implementations;
using Freem.Entities.Events.Consumer.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Events.Consumer.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventsConsumption(
        this IServiceCollection services, Action<EventConsumersBuilder>? builderAction = null)
    {
        var builder = new EventConsumersBuilder(services);
        builderAction?.Invoke(builder);
        
        var collection = builder.BuildTypesCollection();
        
        services.AddTransient<EventConsumersExecutor>();
        services.AddSingleton<IEventConsumerRunner, EventConsumerRunner>();
        services.AddSingleton(collection);
        
        return services;
    }
}
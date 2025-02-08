using Freem.Entities.Events.Consumption.Abstractions;
using Freem.Entities.Events.Consumption.DependencyInjection.Microsoft.Builders;
using Freem.Entities.Events.Consumption.DependencyInjection.Microsoft.Implementations;
using Freem.Entities.Events.Consumption.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Events.Consumption.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventsConsumption(
        this IServiceCollection services, Action<EventConsumersBuilder>? builderAction = null)
    {
        var builder = new EventConsumersBuilder(services);
        builderAction?.Invoke(builder);
        
        var collection = builder.BuildTypesCollection();
        
        services.AddTransient<EventConsumersExecutor>();
        services.AddTransient<IEventConsumerResolver, EventConsumerResolver>();
        services.AddSingleton(collection);
        
        return services;
    }
}
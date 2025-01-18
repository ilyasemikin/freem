using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Bus.Events.Implementations;
using Freem.Entities.Bus.Events.Implementations.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Bus.Events.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static EventConsumersBuilder AddEventConsumers(this IServiceCollection services)
    {
        var builder = new EventConsumersBuilder(services);
        
        services.TryAddSingleton<EventConsumerTypesCollection>(_ => builder.Create());
        services.TryAddTransient<IEventConsumersResolver, MicrosoftDependencyInjectionEventConsumersResolver>();
        
        services.TryAddTransient<IEventProducer, EventProducer>();
        services.TryAddTransient<IEventPublisher, EventPublisher>();
        
        return builder;
    }
}
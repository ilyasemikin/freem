using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Events.Implementations;
using Freem.Entities.UseCases.Events.Implementations.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.Events.DependencyInjection.Microsoft;

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
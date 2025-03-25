using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft;
using Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Builders;
using Freem.Entities.Events.Local.Implementations;
using Freem.Entities.Events.Producer.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Events.Local.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalEventsProcessing(
        this IServiceCollection services, Action<EventConsumersBuilder>? builderAction = null)
    {
        return services
            .AddEventProduction<LocalEventPublisher>()
            .AddEventsConsumption(builderAction);
    }
}
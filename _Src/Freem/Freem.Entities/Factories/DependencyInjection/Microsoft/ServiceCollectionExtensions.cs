using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Events;
using Freem.Entities.Identifiers.Factories.DependencyInjection.Microsoft;
using Freem.Time.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Factories.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventEntityFactories(this IServiceCollection services)
    {
        return services
            .AddGuidEntityIdentifierFactories()
            .AddUtcCurrentTimeGetter()
            .AddFactories();
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.TryAddSingleton<
            IEventEntityFactory<CategoryEvent, Category>, 
            CategoryEventEntityFactory>();
        
        services.TryAddSingleton<
            IEventEntityFactory<RecordEvent, Record>, 
            RecordEventEntityFactory>();

        services.TryAddSingleton<
            IEventEntityFactory<RunningRecordEvent, RunningRecord>, 
            RunningRecordEventEntityFactory>();

        services.TryAddSingleton<
            IEventEntityFactory<TagEvent, Tag>, TagEventEntityFactory>();

        return services;
    }
}
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
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
            IEventEntityFactory<CategoryEvent, EventIdentifier, UserIdentifier, Category, CategoryIdentifier>,
            CategoryEventEntityFactory>();

        services.TryAddSingleton<
            IEventEntityFactory<RecordEvent, EventIdentifier, UserIdentifier, Record, RecordIdentifier>,
            RecordEventEntityFactory>();

        services.TryAddSingleton<
            IEventEntityFactory<RunningRecordEvent, EventIdentifier, UserIdentifier, RunningRecord, UserIdentifier>,
            RunningRecordEventEntityFactory>();

        services.TryAddSingleton<
            IEventEntityFactory<TagEvent, EventIdentifier, UserIdentifier, Tag, TagIdentifier>,
            TagEventEntityFactory>();

        return services;
    }
}
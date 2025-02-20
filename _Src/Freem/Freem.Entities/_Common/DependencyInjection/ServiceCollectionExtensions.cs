using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Events;
using Freem.Entities.Records;
using Freem.Entities.Records.Comparers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Comparers;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Comparers;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users;
using Freem.Entities.Users.Comparers;
using Freem.Entities.Users.Identifiers;
using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Identifiers.Implementations.Generators.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntitiesEqualityComparers(this IServiceCollection services)
    {
        services.TryAddSingleton<IEqualityComparer<Activity>, ActivityEqualityComparer>();
        services.TryAddSingleton<IEqualityComparer<Record>, RecordEqualityComparer>();
        services.TryAddSingleton<IEqualityComparer<RunningRecord>, RunningRecordEqualityComparer>();
        services.TryAddSingleton<IEqualityComparer<Tag>, TagEqualityComparer>();
        services.TryAddSingleton<IEqualityComparer<User>, UserEqualityComparer>();

        return services;
    }

    public static IServiceCollection AddIdentifiersGenerators(this IServiceCollection services)
    {
        services.AddGuidStringIdentifierGenerators()
            .AddGenerator<ActivityIdentifier>(value => new ActivityIdentifier(value))
            .AddGenerator<RecordIdentifier>(value => new RecordIdentifier(value))
            .AddGenerator<RunningRecordIdentifier>(value => new RunningRecordIdentifier(value))
            .AddGenerator<TagIdentifier>(value => new TagIdentifier(value))
            .AddGenerator<UserIdentifier>(value => new UserIdentifier(value))
            .AddGenerator<EventIdentifier>(value => new EventIdentifier(value));
        
        services.AddSingletonExistedService<
            IIdentifierGenerator<IIdentifier>, 
            IIdentifierGenerator<ActivityIdentifier>>();
        services.AddSingletonExistedService<
            IIdentifierGenerator<IIdentifier>, 
            IIdentifierGenerator<RecordIdentifier>>();
        services.AddSingletonExistedService<
            IIdentifierGenerator<IIdentifier>,
            IIdentifierGenerator<RunningRecordIdentifier>>();
        services.AddSingletonExistedService<
            IIdentifierGenerator<IIdentifier>,
            IIdentifierGenerator<TagIdentifier>>();
        services.AddSingletonExistedService<
            IIdentifierGenerator<IIdentifier>,
            IIdentifierGenerator<UserIdentifier>>();
        services.AddSingletonExistedService<
            IIdentifierGenerator<IIdentifier>,
            IIdentifierGenerator<EventIdentifier>>();
        
        return services;
    }

    public static IServiceCollection AddEntitiesIdentifiersNameConverters(this IServiceCollection services)
    {
        services.TryAddSingleton<EntityIdentifierFactory>();
        services.TryAddSingleton<EntityIdentifierNameProvider>();
        
        return services;
    }

    public static IServiceCollection AddEventsServices(this IServiceCollection services)
    {
        services.TryAddSingleton<EventJsonConverter>();
        services.TryAddSingleton<EventsFactory>();
        
        return services;
    }
}
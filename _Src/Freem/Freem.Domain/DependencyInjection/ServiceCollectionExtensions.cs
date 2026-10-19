using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Identifiers.Implementations.Generators.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentifiersGenerators(this IServiceCollection services)
    {
        services.AddGuidStringIdentifierGenerators()
            .AddGenerator<ActivityIdentifier>(value => new ActivityIdentifier(value))
            .AddGenerator<RecordIdentifier>(value => new RecordIdentifier(value))
            .AddGenerator<RunningRecordIdentifier>(value => new RunningRecordIdentifier(value))
            .AddGenerator<TagIdentifier>(value => new TagIdentifier(value))
            .AddGenerator<EventIdentifier>(value => new EventIdentifier(value));
        
        services.AddSingleton<IIdentifierGenerator<IIdentifier>>(provider =>
            provider.GetRequiredService<IIdentifierGenerator<ActivityIdentifier>>());
        services.AddSingleton<IIdentifierGenerator<IIdentifier>>(provider =>
            provider.GetRequiredService<IIdentifierGenerator<RecordIdentifier>>());
        services.AddSingleton<IIdentifierGenerator<IIdentifier>>(provider =>
            provider.GetRequiredService<IIdentifierGenerator<RunningRecordIdentifier>>());
        services.AddSingleton<IIdentifierGenerator<IIdentifier>>(provider =>
            provider.GetRequiredService<IIdentifierGenerator<TagIdentifier>>());
        services.AddSingleton<IIdentifierGenerator<IIdentifier>>(provider =>
            provider.GetRequiredService<IIdentifierGenerator<EventIdentifier>>());
        
        return services;
    }

    public static IServiceCollection AddEntitiesIdentifiersNameConverters(this IServiceCollection services)
    {
        services.TryAddSingleton(EntityIdentifierFactory.Instance);
        services.TryAddSingleton(EntityIdentifierNameProvider.Instance);
        
        return services;
    }

    public static IServiceCollection AddEventsFactory(this IServiceCollection services)
    {
        services.TryAddSingleton(EventsFactory.Instance);
        
        return services;
    }
}
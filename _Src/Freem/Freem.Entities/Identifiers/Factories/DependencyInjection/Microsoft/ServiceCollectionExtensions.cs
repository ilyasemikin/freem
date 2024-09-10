using Freem.Entities.Abstractions.Identifiers.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Identifiers.Factories.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGuidEntityIdentifierFactories(this IServiceCollection services)
    {
        services.TryAddSingleton<IEntityIdentifierFactory<ActivityIdentifier>, GuidActivityIdentifierEntityFactory>();
        services.TryAddSingleton<IEntityIdentifierFactory<RecordIdentifier>, GuidRecordIdentifierEntityFactory>();
        services.TryAddSingleton<IEntityIdentifierFactory<TagIdentifier>, GuidTagIdentifierFactory>();
        services.TryAddSingleton<IEntityIdentifierFactory<UserIdentifier>, GuidUserIdentifierFactory>();
        services.TryAddSingleton<IEntityIdentifierFactory<EventIdentifier>, GuidEventIdentifierFactory>();

        return services;
    }
}
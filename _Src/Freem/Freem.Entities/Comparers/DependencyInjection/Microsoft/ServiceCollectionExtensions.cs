using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Comparers.DependencyInjection.Microsoft;

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
}
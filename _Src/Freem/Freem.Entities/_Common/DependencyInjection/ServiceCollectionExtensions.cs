using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.Records;
using Freem.Entities.Records.Comparers;
using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Comparers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Comparers;
using Freem.Entities.Users;
using Freem.Entities.Users.Comparers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities._Common.DependencyInjection;

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
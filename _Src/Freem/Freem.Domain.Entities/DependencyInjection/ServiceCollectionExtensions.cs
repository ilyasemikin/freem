using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Activities.Comparers;
using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Records.Comparers;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Entities.RunningRecords.Comparers;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.Tags.Comparers;
using Freem.Domain.Entities.Users;
using Freem.Domain.Entities.Users.Comparers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Domain.Entities.DependencyInjection;

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
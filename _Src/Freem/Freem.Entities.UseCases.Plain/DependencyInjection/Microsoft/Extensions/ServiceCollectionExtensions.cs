using Freem.Entities.UseCases.Plain.Implementations;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.Plain.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlainUseCasesExecutors(this IServiceCollection services)
    {
        services.TryAddTransient<ActivitiesPlainExecutor>();
        services.TryAddTransient<TagsPlainExecutor>();
        services.TryAddTransient<RecordsPlainExecutor>();
        services.TryAddTransient<RunningRecordsPlainExecutor>();
        services.TryAddTransient<UsersPasswordPlainExecutor>();
        services.TryAddTransient<UsersTokensPlainExecutor>();
        
        services.TryAddTransient<ActivitiesPlainSyncExecutor>();
        services.TryAddTransient<TagsPlainSyncExecutor>();
        services.TryAddTransient<RecordsPlainSyncExecutor>();
        services.TryAddTransient<RunningRecordsPlainSyncExecutor>();
        services.TryAddTransient<UsersPasswordPlainSyncExecutor>();
        services.TryAddTransient<UsersTokensPlainSyncExecutor>();
        
        services.TryAddTransient<EntitiesPlainExecutors>();
        services.TryAddTransient<EntitiesPlainSyncExecutors>();
        
        return services;
    }
}
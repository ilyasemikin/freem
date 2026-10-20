using Freem.Domain.DependencyInjection;
using Freem.Domain.Entities.DependencyInjection;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Factories;
using Freem.Domain.Storage.PostgreSQL.Implementations.Converters;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Converters;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Activities;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Users;
using Freem.Storage.Transactions.EFCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using ActivityStatusDb = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlStorage(
        this IServiceCollection services, 
        StorageConfiguration configuration)
    {
        return services
            .AddEntitiesEqualityComparers()
            .AddEntityIdNameConverters()
            .AddDatabaseContext(configuration)
            .AddDatabaseContextErrorHandler()
            .AddEventsConverters()
            .AddStorageTransactions<DatabaseContext>()
            .AddRepositories();
    }
    
    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection services, StorageConfiguration configuration)
    {
        services.TryAddSingleton(_ => NpgsqlDataSourceFactory.Create(configuration));
        
        return services.AddDbContext<DatabaseContext>((provider, builder) =>
        {
            var ds = provider.GetRequiredService<NpgsqlDataSource>();
            
            builder
                .UseNpgsql(ds, options =>
                {
                    options.MigrationsAssembly(EnvironmentNames.Migrations.Assembly);
                    options.MigrationsHistoryTable(EnvironmentNames.Migrations.HistoryTable, EnvironmentNames.Schema);
                    options.MapEnum<ActivityStatusDb>(EntitiesNames.Activities.Models.Status, EnvironmentNames.Schema);
                })
                .EnableServiceProviderCaching(configuration.EnableServiceProviderCaching);

            if (configuration.SensitiveDataLogging)
                builder
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            
            if (configuration.Logger is not null)
                builder.LogTo(message => configuration.Logger(message));
        });
    }

    private static IServiceCollection AddDatabaseContextErrorHandler(this IServiceCollection services)
    {
        services.TryAddSingleton<DatabaseColumnToIdPossibleConverter>();

        services.TryAddSingleton<DatabaseForeignKeyConstraintErrorToErrorConverter>();
        services.TryAddSingleton<TriggerConstraintErrorToErrorConverter>();
        services.TryAddSingleton<DuplicateKeyErrorToErrorConverter>();
        
        services.TryAddSingleton<DatabaseContextWriteExceptionHandler>();
        
        return services;
    }

    private static IServiceCollection AddEventsConverters(this IServiceCollection services)
    {
        services.TryAddSingleton<EventEnvelopeToDatabaseEntityConverter>();
        services.TryAddSingleton<DatabaseEntityToEventConverter>();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.TryAddTransient<IUserRepository, UsersRepository>();
        services.TryAddTransient<ITagRepository, TagsRepository>();
        services.TryAddTransient<IRecordRepository, RecordsRepository>();
        services.TryAddTransient<IRunningRecordRepository, RunningRecordsRepository>();
        services.TryAddTransient<IActivityRepository, ActivitiesRepository>();
        services.TryAddTransient<IEventRepository, EventsRepository>();
        
        return services;
    }
}
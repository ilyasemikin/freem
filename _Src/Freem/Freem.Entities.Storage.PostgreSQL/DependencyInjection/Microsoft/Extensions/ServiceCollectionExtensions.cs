using Freem.Domain.DependencyInjection;
using Freem.Domain.Entities.DependencyInjection;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Factories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Converters;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Users;
using Freem.Storage.EFCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ActivityStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlStorage(
        this IServiceCollection services, 
        StorageConfiguration configuration)
    {
        return services
            .AddEntitiesEqualityComparers()
            .AddEntityIdNameConverters()
            .AddEventsFactory()
            .AddDatabaseContext(configuration)
            .AddDatabaseContextErrorHandler()
            .AddEventsConverters()
            .AddStorageTransactions<DatabaseContext>()
            .AddRepositories();
    }
    
    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection services, StorageConfiguration configuration)
    {
        var ds = NpgsqlDataSourceFactory.Create(configuration);
        
        return services.AddDbContext<DatabaseContext>(builder =>
        {
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

        services.TryAddSingleton<DatabaseForeignKeyConstraintErrorToExceptionConverter>();
        services.TryAddSingleton<TriggerConstraintErrorToExceptionConverter>();
        services.TryAddSingleton<DuplicateKeyErrorToExceptionConverter>();
        
        services.TryAddSingleton<DatabaseContextWriteExceptionHandler>();
        
        return services;
    }

    private static IServiceCollection AddEventsConverters(this IServiceCollection services)
    {
        services.TryAddSingleton<EventEntityToDatabaseEntityConverter>();
        services.TryAddSingleton<DatabaseEntityToEventEntityConverter>();
        
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
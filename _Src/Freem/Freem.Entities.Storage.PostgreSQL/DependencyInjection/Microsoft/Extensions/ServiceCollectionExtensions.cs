using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Factories;
using Freem.Entities.Storage.PostgreSQL.Database.Models;
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
using Freem.Entities.Users.Identifiers;
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
            .AddEntitiesIdentifiersNameConverters()
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
        services.TryAddSingleton<DatabaseColumnToIdentifierPossibleConverter>();

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
        services.TryAddRepository<UsersRepository>();
        services.TryAddRepository<TagsRepository>();
        services.TryAddRepository<RecordsRepository>();
        services.TryAddRepository<RunningRecordsRepository>();
        services.TryAddRepository<ActivitiesRepository>();
        services.TryAddRepository<EventsRepository>();
        
        return services;
    }

    private static void TryAddRepository<TRepository>(this IServiceCollection services)
        where TRepository : class
    {
        services.TryAddTransient<TRepository>();

        var interfaces = typeof(TRepository).GetInterfaces();
        foreach (var @interface in interfaces)
        {
            services.TryAddTransient(@interface, provider => provider.GetRequiredService<TRepository>());
        }
    }
}
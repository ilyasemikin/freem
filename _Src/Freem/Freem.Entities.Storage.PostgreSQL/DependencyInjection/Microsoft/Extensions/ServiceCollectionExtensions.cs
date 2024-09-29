using Freem.Converters.Abstractions;
using Freem.Converters.DependencyInjection.Microsoft.Extensions;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities._Common.DependencyInjection;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services, StorageConfiguration configuration)
    {
        return services
            .AddEntitiesEqualityComparers()
            .AddDatabaseContext(configuration)
            .AddDatabaseContextErrorHandler()
            .AddEventsConverters()
            .AddRepositories();
    }
    
    private static IServiceCollection AddDatabaseContext(this IServiceCollection services, StorageConfiguration configuration)
    {
        if (services.ContainsServiceType<DatabaseContext>())
            return services;
        
        var dataSource = NpgsqlDataSourceFactory.Create(configuration.ConnectionString);

        return services.AddDbContext<DatabaseContext>(builder =>
        {
            builder
                .UseNpgsql(dataSource, options =>
                {
                    options.MigrationsAssembly(EnvironmentNames.Migrations.Assembly);
                    options.MigrationsHistoryTable(EnvironmentNames.Migrations.HistoryTable);
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
        services
            .TryAddSingleton<
                IPossibleConverter<DatabaseColumnWithValue, IEntityIdentifier>,
                DatabaseColumnToIdentifierPossibleConverter>();

        services.TryAddSingleton<
            IPossibleConverter<DatabaseContextWriteContext, DatabaseForeignKeyConstraintError, Exception>,
            DatabaseForeignKeyConstraintErrorToExceptionConverter>();
        services.TryAddSingleton<
            IConverter<DatabaseContextWriteContext, TriggerConstraintError, Exception>,
            TriggerConstraintErrorToExceptionConverter>();

        services.AddConvertersCollection<DatabaseContextWriteContext, IDatabaseError, Exception>((provider, builder) => builder
            .Add(provider.GetRequiredService<IPossibleConverter<DatabaseContextWriteContext, DatabaseForeignKeyConstraintError, Exception>>())
            .Add(provider.GetRequiredService<IConverter<DatabaseContextWriteContext, TriggerConstraintError, Exception>>()));
        
        services.TryAddSingleton<DatabaseContextWriteExceptionHandler>();
        
        return services;
    }

    private static IServiceCollection AddEventsConverters(this IServiceCollection services)
    {
        services.TryAddSingleton<
            IConverter<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventEntity>, 
            EventEntityToDatabaseEntityConverter>();
        
        services.TryAddTransient<
            IConverter<EventEntity, IEntityEvent<IEntityIdentifier, UserIdentifier>>,
            DatabaseEntityToEventEntityConverter>();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.TryAddTransient<IUsersRepository, UsersRepository>();
        services.TryAddTransient<ITagsRepository, TagsRepository>();
        services.TryAddTransient<IRecordsRepository, RecordsRepository>();
        services.TryAddTransient<IRunningRecordRepository, RunningRecordsRepository>();
        services.TryAddTransient<IActivitiesRepository, ActivitiesRepository>();
        services.TryAddTransient<IEventsRepository, EventsRepository>();
        
        return services;
    }
}
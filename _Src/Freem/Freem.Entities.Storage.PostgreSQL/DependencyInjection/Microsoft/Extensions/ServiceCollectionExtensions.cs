using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Factories.DependencyInjection.Microsoft;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.Database.Factories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Users;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services, StorageConfiguration configuration)
    {
        return services
            .AddEventEntityFactories()
            .AddDatabaseContext(configuration)
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
                });

            if (configuration.Logger is not null)
                builder.LogTo(message => configuration.Logger(message));
        });
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.TryAddTransient<IUsersRepository, UsersRepository>();
        services.TryAddTransient<ITagsRepository, TagsRepository>();
        services.TryAddTransient<IRecordsRepository, RecordsRepository>();
        services.TryAddTransient<IRunningRecordRepository, RunningRecordsRepository>();
        services.TryAddTransient<ICategoriesRepository, CategoriesRepository>();
        services.TryAddTransient<IEventsRepository, EventsRepository>();
        
        return services;
    }
}
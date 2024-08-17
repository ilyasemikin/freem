using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.Database.Factories;

namespace Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services, StorageConfiguration configuration)
    {
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
}
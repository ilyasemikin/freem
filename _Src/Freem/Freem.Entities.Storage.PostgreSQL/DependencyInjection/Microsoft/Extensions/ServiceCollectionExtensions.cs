using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CategoryStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.CategoryStatus;

namespace Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services, string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<CategoryStatusDb>();

        var dataSource = dataSourceBuilder.Build();

        return services.AddDbContext<DatabaseContext>(builder => builder
            .UseNpgsql(dataSource, options =>
            {
                options.MigrationsAssembly(EnvironmentNames.Migrations.Assembly);
                options.MigrationsHistoryTable(EnvironmentNames.Migrations.HistoryTable);
            })
        );
    }
}

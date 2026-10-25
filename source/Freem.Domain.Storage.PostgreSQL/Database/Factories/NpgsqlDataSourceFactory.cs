using Freem.Domain.Storage.PostgreSQL.Configuration;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection;
using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Npgsql;
using ActivityStatusDb = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Domain.Storage.PostgreSQL.Database.Factories;

internal static class NpgsqlDataSourceFactory
{
    public static NpgsqlDataSource Create(PostgresConfiguration configuration, StorageOptions options)
    {
        var builder = new NpgsqlDataSourceBuilder(configuration.ConnectionString);
        
        builder.MapEnum<ActivityStatusDb>($"{EnvironmentNames.Schema}.{EntitiesNames.Activities.Models.Status}");
        
        if (configuration.EnableSensitiveDataLogging)
            builder.EnableParameterLogging();
        
        if (options.LoggerFactory is not null)
            builder.UseLoggerFactory(options.LoggerFactory);

        return builder.Build();
    }
}
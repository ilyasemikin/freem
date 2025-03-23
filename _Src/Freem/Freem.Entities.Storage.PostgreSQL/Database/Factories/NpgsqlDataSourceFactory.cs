using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Npgsql;
using ActivityStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.Database.Factories;

internal static class NpgsqlDataSourceFactory
{
    public static NpgsqlDataSource Create(StorageConfiguration configuration)
    {
        var builder = new NpgsqlDataSourceBuilder(configuration.ConnectionString);
        
        builder.MapEnum<ActivityStatusDb>();
        
        if (configuration.SensitiveDataLogging)
            builder.EnableParameterLogging();
        
        if (configuration.LoggerFactory is not null)
            builder.UseLoggerFactory(configuration.LoggerFactory);

        return builder.Build();
    }
}
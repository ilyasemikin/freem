using Npgsql;
using ActivityStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;
using EventActionDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base.EventAction;

namespace Freem.Entities.Storage.PostgreSQL.Database.Factories;

internal static class NpgsqlDataSourceFactory
{
    public static NpgsqlDataSource Create(string connectionString)
    {
        var builder = new NpgsqlDataSourceBuilder(connectionString);
        
        builder.MapEnum<ActivityStatusDb>();
        
        builder.MapEnum<EventActionDb>();

        return builder.Build();
    }
}
using Npgsql;
using CategoryStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.CategoryStatus;

namespace Freem.Entities.Storage.PostgreSQL.Database.Factories;

internal static class NpgsqlDataSourceFactory
{
    public static NpgsqlDataSource Create(string connectionString)
    {
        var builder = new NpgsqlDataSourceBuilder(connectionString);
        builder.MapEnum<CategoryStatusDb>();

        return builder.Build();
    }
}
using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;

namespace Freem.Domain.Storage.PostgreSQL.Database.Extensions;

internal static class DatabaseContextExtensions
{
    private const string TestDatabaseSuffix = "_test";

    public static bool IsTestDatabaseName(string? name)
    {
        return name is not null && name.EndsWith(TestDatabaseSuffix, StringComparison.Ordinal);
    }

    public static void TruncateTables(this DatabaseContext context)
    {
        var databaseName = context.Database.GetDbConnection().Database;
        if (!IsTestDatabaseName(databaseName))
            throw new InvalidOperationException(
                $"Tables can be truncated only in a database whose name ends with '{TestDatabaseSuffix}', got '{databaseName}'.");

        context.Database.ExecuteSqlRaw($"""

                                        BEGIN;

                                        TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Users.Table} CASCADE;
                                        TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Tags.Table} CASCADE;
                                        TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Activities.Table} CASCADE;
                                        TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Records.Table} CASCADE;
                                        TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.RunningRecords.Table} CASCADE;
                                        TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Events.Table} CASCADE;

                                        COMMIT;
                                        """);
    }
}
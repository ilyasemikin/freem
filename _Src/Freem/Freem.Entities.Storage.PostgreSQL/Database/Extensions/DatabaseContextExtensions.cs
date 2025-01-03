using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Extensions;

internal static class DatabaseContextExtensions
{
    public static void TruncateTables(this DatabaseContext context)
    {
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
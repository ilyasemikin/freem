using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Base;

[Collection("Sequential")]
public abstract class ConstraintTriggerTestsBase : IDisposable
{
    internal DatabaseContext Context { get; }

    public ConstraintTriggerTestsBase()
    {
        Context = DatabaseContextFactory.Create();
    }

    public void Dispose()
    {
        Context.Database.ExecuteSqlRaw($@"
BEGIN;

TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Users.Table} CASCADE;
TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Tags.Table} CASCADE;
TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Categories.Table} CASCADE;
TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.Records.Table} CASCADE;
TRUNCATE TABLE {EnvironmentNames.Schema}.{EntitiesNames.RunningRecords.Table} CASCADE;

COMMIT;");

        GC.SuppressFinalize(this);
    }
}

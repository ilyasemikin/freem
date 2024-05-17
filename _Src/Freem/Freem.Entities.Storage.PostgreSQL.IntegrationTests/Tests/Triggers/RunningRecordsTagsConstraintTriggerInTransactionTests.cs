using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers;

public sealed class RunningRecordsTagsConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    [Fact]
    public async Task RunningRecordsTags_ShouldExecute_WhenUserIdsCorrect()
    {
        var entities = UserEntities.CreateSecondUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddRunningRecordAsync(Context);
        await entities.AddTagsAsync(Context);

        var relation = new RunningRecordTagRelationEntity
        {
            RunningRecordId = entities.RunningRecord.UserId,
            TagId = entities.Tags[0].Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task RunningRecordsTags_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var first = UserEntities.CreateFirstUserEntities();
        var second = UserEntities.CreateSecondUserEntities();

        await first.AddUserAsync(Context);
        await first.AddRunningRecordAsync(Context);

        await second.AddUserAsync(Context);
        await second.AddTagsAsync(Context);

        var relation = new RunningRecordTagRelationEntity
        {
            RunningRecordId = first.RunningRecord.UserId,
            TagId = second.Tags[0].Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must be equils"));
    }
}

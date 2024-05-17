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

public class RunningRecordsCategoriesConstraintTriggerTests : ConstraintTriggerTestsBase
{
    [Fact]
    public async Task RunningRecordsCategories_ShouldThrowException_WhenNoCategoriesAdded()
    {
        var entities = UserEntities.CreateSecondUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddCategoriesAsync(Context);

        var record = entities.RunningRecord;
        record.Categories = null;

        await Context.RunningRecords.AddAsync(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have at least one category"));
    }

    [Fact]
    public async Task RunningRecordsCategories_ShouldThrowException_WhenDeleteLastCategory()
    {
        var entities = UserEntities.CreateFirstUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddCategoriesAsync(Context);

        var record = entities.RunningRecord;
        record.Categories = [entities.Categories[0]];

        await Context.RunningRecords.AddAsync(record);

        await Context.SaveChangesAsync();

        record = await Context.RunningRecords
            .Include(e => e.Categories)
            .FirstAsync(e => e.UserId == record.UserId);
        record.Categories = null;

        Context.RunningRecords.Update(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have at least one category"));
    }
}

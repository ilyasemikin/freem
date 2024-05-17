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

public sealed class RecordsCategoriesConstraintTriggerTests : ConstraintTriggerTestsBase
{
    [Fact]
    public async Task RecordsCategories_ShouldThrowException_WhenNoCategoriesAdded()
    {
        var entities = UserEntities.CreateFirstUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddCategoriesAsync(Context);

        var record = entities.Records[0];
        record.Categories = null;

        await Context.Records.AddAsync(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have at least one category"));
    }

    [Fact]
    public async Task RecordsCategories_ShouldThrowException_WhenDeleteLastCategory()
    {
        var entities = UserEntities.CreateFirstUserEntities();

        await entities.AddUserAsync(Context);
        await entities.AddCategoriesAsync(Context);

        var record = entities.Records[0];
        record.Categories = [entities.Categories[0]];

        await Context.Records.AddAsync(record);

        await Context.SaveChangesAsync();

        record = await Context.Records
            .Include(e => e.Categories)
            .FirstAsync(e => e.Id == record.Id);
        record.Categories = null;

        Context.Records.Update(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have at least one category"));
    }
}

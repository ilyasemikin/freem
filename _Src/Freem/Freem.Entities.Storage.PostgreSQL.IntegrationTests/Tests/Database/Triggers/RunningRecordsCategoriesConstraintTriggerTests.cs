using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class RunningRecordsCategoriesConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RunningRecordsCategoriesConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RunningRecordsCategories_ShouldThrowException_WhenNoCategoriesAdded()
    {
        var factory = EntitiesFactory.CreateSecondUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);

        var record = factory.CreateRunningRecord();

        await Context.RunningRecords.AddAsync(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have categories count in range"));
    }

    [Fact]
    public async Task RunningRecordsCategories_ShouldThrowException_WhenDeleteLastCategory()
    {
        var factory = EntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);

        var record = factory.CreateRunningRecord();
        record.Categories = new List<CategoryEntity> { category };

        await Context.RunningRecords.AddAsync(record);

        await Context.SaveChangesAsync();

        record = await Context.RunningRecords
            .Include(e => e.Categories)
            .FirstAsync(e => e.UserId == record.UserId);
        record.Categories = null;

        Context.RunningRecords.Update(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains("P0001") && e.Message.Contains("must have categories count in range"));
    }
}

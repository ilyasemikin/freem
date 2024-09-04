using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public sealed class RunningRecordsCategoriesConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    public RunningRecordsCategoriesConstraintTriggerInTransactionTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RunningRecordsCategories_ShouldExecute_WhenUserIdsCorrect()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);

        var record = factory.CreateRunningRecord();

        await Context.RunningRecords.AddAsync(record);

        var relation = new RunningRecordCategoryRelationEntity
        {
            RunningRecordUserId = record.UserId,
            CategoryId = category.Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task RunningRecordsCategories_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var firstUser = firstFactory.User;
        var firstCategory = firstFactory.CreateCategory();

        var secondUser = secondFactory.User;
        var secondCategory = secondFactory.CreateCategory();

        await Context.Users.AddAsync(firstUser);
        await Context.Categories.AddAsync(firstCategory);

        await Context.Users.AddAsync(secondUser);
        await Context.Categories.AddAsync(secondCategory);

        var record = firstFactory.CreateRunningRecord();

        await Context.RunningRecords.AddAsync(record);

        var relations = new[]
        {
            new RunningRecordCategoryRelationEntity
            {
                RunningRecordUserId = record.UserId,
                CategoryId = firstCategory.Id
            },
            new RunningRecordCategoryRelationEntity
            {
                RunningRecordUserId = record.UserId,
                CategoryId = secondCategory.Id
            }
        };

        await Context.AddRangeAsync(relations);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RunningRecordsCategoriesDifferentUserIds));
    }
}

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

public sealed class RecordsActivitiesConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    public RecordsActivitiesConstraintTriggerInTransactionTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RecordsActivities_ShouldExecute_WhenUserIdsCorrect()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var record = factory.CreateRecord();

        await Context.Records.AddAsync(record);

        var relation = new RecordActivityRelationEntity
        {
            RecordId = record.Id,
            ActivityId = activity.Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task RecordsActivities_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var firstUser = firstFactory.User;
        var firstActivity = firstFactory.CreateActivity();

        var secondUser = secondFactory.User;
        var secondActivity = secondFactory.CreateActivity();

        var firstRecord = firstFactory.CreateRecord();

        await Context.Users.AddAsync(firstUser);
        await Context.Activities.AddAsync(firstActivity);

        await Context.Users.AddAsync(secondUser);
        await Context.Activities.AddAsync(secondActivity);

        await Context.Records.AddAsync(firstRecord);

        var relations = new[]
        {
            new RecordActivityRelationEntity
            {
                RecordId = firstRecord.Id,
                ActivityId = firstActivity.Id
            },
            new RecordActivityRelationEntity
            {
                RecordId = firstRecord.Id,
                ActivityId = secondActivity.Id
            }
        };

        await Context.AddRangeAsync(relations);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsActivitiesDifferentUserIds));
    }
}

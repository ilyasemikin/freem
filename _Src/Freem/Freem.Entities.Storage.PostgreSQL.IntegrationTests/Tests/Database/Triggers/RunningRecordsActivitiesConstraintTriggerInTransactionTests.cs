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

public sealed class RunningRecordsActivitiesConstraintTriggerInTransactionTests : ConstraintTriggerInTransactionTestsBase
{
    public RunningRecordsActivitiesConstraintTriggerInTransactionTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RunningRecordsActivities_ShouldExecute_WhenUserIdsCorrect()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var record = factory.CreateRunningRecord();

        await Context.RunningRecords.AddAsync(record);

        var relation = new RunningRecordActivityRelationEntity
        {
            RunningRecordUserId = record.UserId,
            ActivityId = activity.Id
        };

        await Context.AddAsync(relation);

        await Context.ShouldNotThrowExceptionAsync();
    }

    [Fact]
    public async Task RunningRecordsActivities_ShouldThrowException_WhenHaveDifferentUserIds()
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var firstUser = firstFactory.User;
        var firstActivity = firstFactory.CreateActivity();

        var secondUser = secondFactory.User;
        var secondActivity = secondFactory.CreateActivity();

        await Context.Users.AddAsync(firstUser);
        await Context.Activities.AddAsync(firstActivity);

        await Context.Users.AddAsync(secondUser);
        await Context.Activities.AddAsync(secondActivity);

        var record = firstFactory.CreateRunningRecord();

        await Context.RunningRecords.AddAsync(record);

        var relations = new[]
        {
            new RunningRecordActivityRelationEntity
            {
                RunningRecordUserId = record.UserId,
                ActivityId = firstActivity.Id
            },
            new RunningRecordActivityRelationEntity
            {
                RunningRecordUserId = record.UserId,
                ActivityId = secondActivity.Id
            }
        };

        await Context.AddRangeAsync(relations);

        await Context.ShouldThrowExceptionAsync<DbUpdateException, PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds));
    }
}

using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class RunningRecordsActivitiesConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RunningRecordsActivitiesConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RunningRecordsActivities_ShouldThrowException_WhenNoActivitiesAdded()
    {
        var factory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var record = factory.CreateRunningRecord();

        await Context.RunningRecords.AddAsync(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RunningRecordsActivitiesInvalidCount));
    }

    [Fact]
    public async Task RunningRecordsActivities_ShouldThrowException_WhenDeleteLastActivity()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var record = factory.CreateRunningRecord();
        record.Activities = new List<ActivityEntity> { activity };

        await Context.RunningRecords.AddAsync(record);

        await Context.SaveChangesAsync();

        record = await Context.RunningRecords
            .Include(e => e.Activities)
            .FirstAsync(e => e.UserId == record.UserId);
        record.Activities = null;

        Context.RunningRecords.Update(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RunningRecordsActivitiesInvalidCount));
    }
}

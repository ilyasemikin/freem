using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
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

public sealed class RecordsActivitiesConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RecordsActivitiesConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RecordsActivities_ShouldThrowException_WhenNoActivityAdded()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var record = factory.CreateRecord();
        record.Activities = null;

        await Context.Records.AddAsync(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsActivitiesInvalidCount));
    }

    [Fact]
    public async Task RecordsActivities_ShouldThrowException_WhenDeleteLastActivity()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var record = factory.CreateRecord();
        record.Activities = new List<ActivityEntity>() { activity };

        await Context.Records.AddAsync(record);

        await Context.SaveChangesAsync();

        record = await Context.Records
            .Include(e => e.Activities)
            .FirstAsync(e => e.Id == record.Id);
        record.Activities = null;

        Context.Records.Update(record);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsActivitiesInvalidCount));
    }

    [Fact]
    public async Task RecordActivities_ShouldThrowException_WhenAddToManyTags()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var record = factory.CreateRecord();
        var activities = factory.CreateActivities(RelatedActivitiesCollection.MaxActivitiesCount + 1);

        record.Activities = new List<ActivityEntity>() { activities[0] };

        await Context.Users.AddAsync(user);
        await Context.Records.AddAsync(record);
        await Context.Activities.AddRangeAsync(activities);

        await Context.SaveChangesAsync();

        var relations = activities
            .Skip(1)
            .Select(e => new RecordActivityRelationEntity
            {
                RecordId = record.Id,
                ActivityId = e.Id
            });

        await Context.AddRangeAsync(relations);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsActivitiesInvalidCount));
    }
}

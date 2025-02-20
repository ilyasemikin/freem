using Freem.Entities.Activities;
using Freem.Entities.Activities.Events;
using Freem.Entities.Records.Events;
using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Events;
using Freem.Entities.Users;
using Freem.Entities.Users.Events;
using Npgsql;
using Xunit;
using Xunit.Abstractions;
using Record = Freem.Entities.Records.Record;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public class EventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public EventsConstraintTriggerTests(ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task Events_ShouldThrowException_WhenUserIsNotExists()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.AddRangeAsync(user, activity);
        
        var entity = new EventEntity
        {
            Id = "id",
            EntityId = activity.Id,
            EntityName = Activity.EntityName,
            Action = ActivityEventActions.Created,
            UserId = "not_existed_user_id",
        };
        
        await Context.Events.AddAsync(entity);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.EventsRelatedEntityNotExists));
    }

    [Theory]
    [InlineData(Activity.EntityName, ActivityEventActions.Created)]
    [InlineData(Record.EntityName, RecordEventActions.Created)]
    [InlineData(RunningRecord.EntityName, RunningRecordEventActions.Started)]
    [InlineData(Tag.EntityName, TagEventActions.Created)]
    [InlineData(User.EntityName, UserEventActions.SignedIn)]
    public async Task Events_ShouldThrowException_WhenRelatedEntityNotExists(string name, string action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        
        var user = factory.User;

        await Context.AddAsync(user);

        var entity = new EventEntity
        {
            Id = "id",
            EntityId = "not_existed_id",
            EntityName = name,
            Action = action,
            UserId = user.Id
        };
        
        await Context.Events.AddAsync(entity);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => 
                e.Message.Contains(TriggerErrorCodes.EventsRelatedEntityNotExists) && 
                e.Message.Contains(name) && 
                e.Message.Contains(entity.EntityId));
    }
}
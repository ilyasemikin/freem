using Freem.Domain;
using Freem.Domain.Activities.Events;
using Freem.Domain.Records.Events;
using Freem.Domain.RunningRecords.Events;
using Freem.Domain.Tags.Events;
using Freem.Domain.Users.Events;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

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
            EntityName = EntityNames.Activity,
            Action = ActivityEventActions.Created,
            UserId = "not_existed_user_id",
        };
        
        await Context.Events.AddAsync(entity);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.EventsRelatedEntityNotExists));
    }

    [Theory]
    [InlineData(EntityNames.Activity, ActivityEventActions.Created)]
    [InlineData(EntityNames.Record, RecordEventActions.Created)]
    [InlineData(EntityNames.RunningRecord, RunningRecordEventActions.Started)]
    [InlineData(EntityNames.Tag, TagEventActions.Created)]
    [InlineData(EntityNames.User, UserEventActions.Registered)]
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
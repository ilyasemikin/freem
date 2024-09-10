using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
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

public sealed class RunningRecordEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RunningRecordEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    [InlineData(EventAction.Removed)]
    public async Task RunningRecordEvent_ShouldSuccess_WhenRunningRecordExists(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();
        var record = factory.CreateRunningRecord();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);
        await Context.RunningRecords.AddAsync(record);

        var relation = new RunningRecordActivityRelationEntity
        {
            RunningRecordUserId = record.UserId,
            ActivityId = activity.Id
        };

        await Context.AddAsync(relation);
        
        var @event = new RunningRecordEventEntity
        {
            Id = "id",
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Fact]
    public async Task RunningRecordEvent_ShouldSuccess_WhenRunningRecordNotExistsAndActionIsRemoved()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.Users.AddAsync(user);

        var @event = new RunningRecordEventEntity
        {
            Id = "id",
            UserId = user.Id,
            Action = EventAction.Removed,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    public async Task RunningRecordEvent_ShouldThrowException_WhenUserDoesNotExist(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        
        var user = factory.User;

        await Context.Users.AddAsync(user);
        
        var @event = new RunningRecordEventEntity
        {
            Id = "id",
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RunningRecordsEventsUserNotExist));
    }
}
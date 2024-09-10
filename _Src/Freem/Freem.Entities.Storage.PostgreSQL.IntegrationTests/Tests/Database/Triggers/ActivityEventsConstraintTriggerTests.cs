using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public sealed class ActivityEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public ActivityEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    [InlineData(EventAction.Removed)]
    public async Task ActivityEvent_ShouldSuccess_WhenActivityExists(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var activity = factory.CreateActivity();

        await Context.Users.AddAsync(user);
        await Context.Activities.AddAsync(activity);

        var @event = new ActivityEventEntity
        {
            Id = "id",
            ActivityId = activity.Id,
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Fact]
    public async Task ActivityEvent_ShouldSuccess_WhenActivityNotExistsAndActionIsRemoved()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.Users.AddAsync(user);

        var @event = new ActivityEventEntity
        {
            Id = "id",
            ActivityId = "not_existed_id",
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
    public async Task ActivityEvent_ShouldThrowException_WhenActivityDoesNotExist(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        
        var user = factory.User;

        await Context.Users.AddAsync(user);
        
        var @event = new ActivityEventEntity
        {
            Id = "id",
            ActivityId = "activity_id",
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.ActivitiesEventsActivityNotExist));
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    public async Task ActivityEvent_ShouldThrowException_WhenActivityAndEventHaveDifferentUserIds(EventAction action)
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();
        
        var firstUser = firstFactory.User;
        var activity = firstFactory.CreateActivity();

        await Context.Users.AddAsync(firstUser);
        await Context.Activities.AddAsync(activity);
        
        var secondUser = secondFactory.User;
        
        await Context.Users.AddAsync(secondUser);

        var @event = new ActivityEventEntity
        {
            Id = "id",
            ActivityId = activity.Id,
            UserId = secondUser.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);
        
        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.ActivitiesEventsDifferentUserIds));
    }
}
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

public sealed class TagEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public TagEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    [InlineData(EventAction.Removed)]
    public async Task TagEvent_ShouldSuccess_WhenTagExists(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var tag = factory.CreateTag();

        await Context.Users.AddAsync(user);
        await Context.Tags.AddAsync(tag);

        var @event = new TagEventEntity
        {
            Id = "id",
            TagId = tag.Id,
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Fact]
    public async Task TagEvent_ShouldSuccess_WhenTagNotExistsAndActionIsRemoved()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.Users.AddAsync(user);

        var @event = new TagEventEntity
        {
            Id = "id",
            TagId = "not_existed_id",
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
    public async Task TagEvent_ShouldThrowException_WhenActivityDoesNotExist(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        
        var user = factory.User;

        await Context.Users.AddAsync(user);
        
        var @event = new TagEventEntity
        {
            Id = "id",
            TagId = "tag_id",
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.TagsEventsTagNotExist));
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    public async Task TagEvent_ShouldThrowException_WhenTagAndEventHaveDifferentIds(EventAction action)
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();
        
        var firstUser = firstFactory.User;
        var tag = firstFactory.CreateTag();
        
        await Context.Users.AddAsync(firstUser);
        await Context.Tags.AddAsync(tag);
        
        var secondUser = secondFactory.User;
        
        await Context.Users.AddAsync(secondUser);

        var @event = new TagEventEntity
        {
            Id = "id",
            TagId = tag.Id,
            UserId = secondUser.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);
        
        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.TagsEventsDifferentUserIds));
    }
}
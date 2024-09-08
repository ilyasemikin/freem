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

public sealed class RecordEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public RecordEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    [InlineData(EventAction.Removed)]
    public async Task RecordEvent_ShouldSuccess_WhenRecordExists(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();
        var record = factory.CreateRecord();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);
        await Context.Records.AddAsync(record);

        var relation = new RecordCategoryRelationEntity
        {
            RecordId = record.Id,
            CategoryId = category.Id
        };

        await Context.AddAsync(relation);
        
        var @event = new RecordEventEntity
        {
            Id = "id",
            RecordId = record.Id,
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Fact]
    public async Task RecordEvent_ShouldSuccess_WhenRecordNotExistsAndActionIsRemoved()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.Users.AddAsync(user);

        var @event = new RecordEventEntity
        {
            Id = "id",
            RecordId = "not_existed_id",
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
    public async Task RecordEvent_ShouldThrowException_WhenRecordDoesNotExist(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        
        var user = factory.User;

        await Context.Users.AddAsync(user);
        
        var @event = new RecordEventEntity
        {
            Id = "id",
            RecordId = "record_id",
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsEventsRecordNotExist));
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    public async Task RecordEvent_ShouldThrowException_WhenRecordAndEventHaveDifferentUserIds(EventAction action)
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();
        
        var firstUser = firstFactory.User;
        var category = firstFactory.CreateCategory();
        var record = firstFactory.CreateRecord();

        var relation = new RecordCategoryRelationEntity
        {
            RecordId = record.Id,
            CategoryId = category.Id
        };
        
        await Context.Users.AddAsync(firstUser);
        await Context.Categories.AddAsync(category);
        await Context.Records.AddAsync(record);
        await Context.AddAsync(relation);
        
        var secondUser = secondFactory.User;
        
        await Context.Users.AddAsync(secondUser);

        var @event = new RecordEventEntity
        {
            Id = "id",
            RecordId = record.Id,
            UserId = secondUser.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.RecordsEventsDifferentUserIds));
    }
}
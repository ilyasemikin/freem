using Freem.Domain.Activities;
using Freem.Domain.Activities.Events.Arhived;
using Freem.Domain.Activities.Events.Created;
using Freem.Domain.Activities.Events.Removed;
using Freem.Domain.Activities.Events.Unarchived;
using Freem.Domain.Activities.Events.Updated;
using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Events.Created;
using Freem.Domain.Records.Events.Removed;
using Freem.Domain.Records.Events.Updated;
using Freem.Domain.RunningRecords;
using Freem.Domain.RunningRecords.Events.Removed;
using Freem.Domain.RunningRecords.Events.Started;
using Freem.Domain.RunningRecords.Events.Stopped;
using Freem.Domain.RunningRecords.Events.Updated;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Events.Created;
using Freem.Domain.Tags.Events.Removed;
using Freem.Domain.Tags.Events.Updated;
using Freem.Domain.Users;
using Freem.Domain.Users.Events.Registered;
using Freem.Domain.Users.Events.SettingsChanged;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Utilities.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class EventsRepositoryTests : BaseRepositoryTests<IEventRepository>
{
    public EventsRepositoryTests(
        ITestOutputHelper output) 
        : base(output)
    {
    }

    public delegate IEntityEvent<ActivityId, UserId> ActivityEventGenerator(
        EventId id,
        ActivityId activityId, 
        UserId userId);

    public delegate IEntityEvent<RecordId, UserId> RecordEventGenerator(
        EventId id,
        RecordId recordId,
        UserId userId);

    public delegate IEntityEvent<RunningRecordId, UserId> RunningRecordEventGenerator(
        EventId id,
        RunningRecordId runningRecordId);

    public delegate IEntityEvent<TagId, UserId> TagEventGenerator(
        EventId id,
        TagId tagId,
        UserId userId);
    
    public delegate IEntityEvent<UserId, UserId> UserEventGenerator(
        EventId id,
        UserId userId);
    
    public static TheoryData<ActivityEventGenerator> ValidActivityEntitiesCases =>
        new()
        {
            (id, entityId, userId) => new ActivityCreatedEvent(id, entityId, userId),
            (id, entityId, userId) => new ActivityUpdatedEvent(id, entityId, userId),
            (id, entityId, userId) => new ActivityRemovedEvent(id, entityId, userId),
            (id, entityId, userId) => new ActivityArchivedEvent(id, entityId, userId),
            (id, entityId, userId) => new ActivityUnarchivedEvent(id, entityId, userId),
        };

    [Theory]
    [MemberData(nameof(ValidActivityEntitiesCases))]
    public async Task CreateAsync_ShouldSuccess_WhenPassActivityEvent(ActivityEventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();

        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();

        var id = EventId.Generate();
        var userId = UserId.Parse(dbUser.Id, null);
        var entityId = ActivityId.Parse(dbActivity.Id, null);
        
        var @event = factory(id, entityId, userId);
        var eventType = @event.GetType();
        
        // Act
        await Repository.CreateAsync(@event);

        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        Assert.Equal(eventType, result.Entity.GetType());
        Assert.Equal(id, result.Entity.Id);
        Assert.Equal(entityId, result.Entity.EntityId);
        Assert.Equal(userId, result.Entity.UserEntityId);
    }

    public static TheoryData<RecordEventGenerator> ValidRecordEntitiesCases =>
        new()
        {
            (id, entityId, userId) => new RecordCreatedEvent(id, entityId, userId),
            (id, entityId, userId) => new RecordUpdatedEvent(id, entityId, userId),
            (id, entityId, userId) => new RecordRemovedEvent(id, entityId, userId),
        };
    
    [Theory]
    [MemberData(nameof(ValidRecordEntitiesCases))]
    public async Task CreateAsync_ShouldSuccess_WhenPassRecordEvent(RecordEventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.Activities.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var id = EventId.Generate();
        var userId = UserId.Parse(dbUser.Id, null);
        var entityId = RecordId.Parse(dbRecord.Id, null);
        
        var @event = factory(id, entityId, userId);
        var eventType = @event.GetType();
        
        // Act
        await Repository.CreateAsync(@event);
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        Assert.Equal(eventType, result.Entity.GetType());
        Assert.Equal(id, result.Entity.Id);
        Assert.Equal(entityId, result.Entity.EntityId);
        Assert.Equal(userId, result.Entity.UserEntityId);
    }

    public static TheoryData<RunningRecordEventGenerator> ValidRunningRecordEventsCases =>
        new()
        {
            (id, recordId) => new RunningRecordStartedEvent(id, recordId),
            (id, recordId) => new RunningRecordStoppedEvent(id, recordId),
            (id, recordId) => new RunningRecordUpdatedEvent(id, recordId),
            (id, recordId) => new RunningRecordRemovedEvent(id, recordId),
        };

    [Theory]
    [MemberData(nameof(ValidRunningRecordEventsCases))]
    public async Task CreateAsync_ShouldSuccess_WhenPassRunningRecordEvent(RunningRecordEventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.Activities.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var id = EventId.Generate();
        var entityId = RunningRecordId.Parse(dbRecord.UserId, null);

        var @event = factory(id, entityId);
        var eventType = @event.GetType();
        
        // Act
        await Repository.CreateAsync(@event);
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        Assert.Equal(eventType, result.Entity.GetType());
        Assert.Equal(id, result.Entity.Id);
        var expectedUserId = entityId.ToUserId();
        Assert.Equal(entityId, result.Entity.EntityId);
        Assert.Equal(expectedUserId, result.Entity.UserEntityId);
    }

    public static TheoryData<TagEventGenerator> ValidTagEventsCase =>
        new()
        {
            (id, tagId, userId) => new TagCreatedEvent(id, tagId, userId),
            (id, tagId, userId) => new TagUpdatedEvent(id, tagId, userId),
            (id, tagId, userId) => new TagRemovedEvent(id, tagId, userId),
        };

    [Theory]
    [MemberData(nameof(ValidTagEventsCase))]
    public async Task CreateAsync_ShouldSuccess_WhenPassTagEvent(TagEventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        
        var id = EventId.Generate();
        var entityId = TagId.Parse(dbTag.Id, null);
        var userId = UserId.Parse(dbUser.Id, null);
        
        var @event = factory(id, entityId, userId);
        var eventType = @event.GetType();
        
        // Act
        await Repository.CreateAsync(@event);
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        Assert.Equal(eventType, result.Entity.GetType());
        Assert.Equal(id, result.Entity.Id);
        Assert.Equal(entityId, result.Entity.EntityId);
        Assert.Equal(userId, result.Entity.UserEntityId);
    }

    public static TheoryData<UserEventGenerator> ValidUserEventsCase =>
        new()
        {
            (id, userId) => new UserRegisteredEvent(id, userId),
            (id, userId) => new UserSettingsChanged(id, userId),
        };

    [Theory]
    [MemberData(nameof(ValidUserEventsCase))]
    public async Task CreateAsync_ShouldSuccess_WhenPassUserEvent(UserEventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        
        await Database.Users.AddAsync(dbUser);
        await Database.SaveChangesAsync();
        
        var id = EventId.Generate();
        var userId = UserId.Parse(dbUser.Id, null);
        
        var @event = factory(id, userId);
        var eventType = @event.GetType();
        
        // Act
        await Repository.CreateAsync(@event);

        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        Assert.Equal(eventType, result.Entity.GetType());
        Assert.Equal(id, result.Entity.Id);
        Assert.Equal(userId, result.Entity.EntityId);
        Assert.Equal(userId, result.Entity.UserEntityId);
    }

    [Fact]
    public void Events_ShouldAllProcessed()
    {
        // Arrange
        var types = new List<Type>();
        
        var id = EventId.Generate();
        var userId = GenerateUserId();

        var activityId = ActivityId.Generate();
        foreach (var @case in ValidActivityEntitiesCases)
        {
            var generator = @case;
            var @event = generator(id, activityId, userId);
            
            types.Add(@event.GetType());
        }
        
        var recordId = RecordId.Generate();
        foreach (var @case in ValidRecordEntitiesCases)
        {
            var generator = @case;
            var @event = generator(id, recordId, userId);
            
            types.Add(@event.GetType());
        }

        var runningRecordId = RunningRecordId.FromUserId(userId);
        foreach (var @case in ValidRunningRecordEventsCases)
        {
            var generator = @case;
            var @event = generator(id, runningRecordId);
            
            types.Add(@event.GetType());
        }
        
        var tagId = TagId.Generate();
        foreach (var @case in ValidTagEventsCase)
        {
            var generator = @case;
            var @event = generator(id, tagId, userId);
            
            types.Add(@event.GetType());
        }

        foreach (var @case in ValidUserEventsCase)
        {
            var generator = @case;
            var @event = generator(id, userId);
            
            types.Add(@event.GetType());
        }

        var assembly = typeof(IEntityEvent<,>).Assembly;
        var eventTypes = assembly.ExportedTypes.Where(type => 
            !type.IsAbstract && type.GetInterface("IEntityEvent`2") is not null);

        // Act
        var result = types.UnorderedEquals(eventTypes, EqualityComparer<Type>.Default);
        
        // Assert
        Assert.True(result);
    }
}
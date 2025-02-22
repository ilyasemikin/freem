using Freem.Collections.Extensions;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Activities.Events.Arhived;
using Freem.Entities.Activities.Events.Created;
using Freem.Entities.Activities.Events.Removed;
using Freem.Entities.Activities.Events.Unarchived;
using Freem.Entities.Activities.Events.Updated;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Records.Events.Created;
using Freem.Entities.Records.Events.Removed;
using Freem.Entities.Records.Events.Updated;
using Freem.Entities.RunningRecords.Events.Removed;
using Freem.Entities.RunningRecords.Events.Started;
using Freem.Entities.RunningRecords.Events.Stopped;
using Freem.Entities.RunningRecords.Events.Updated;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Entities.Tags.Events.Created;
using Freem.Entities.Tags.Events.Removed;
using Freem.Entities.Tags.Events.Updated;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Events.PasswordCredentialsChanged;
using Freem.Entities.Users.Events.Registered;
using Freem.Entities.Users.Events.SettingsChanged;
using Freem.Entities.Users.Events.SignedIn;
using Freem.Entities.Users.Events.TelegramIntegrationChanged;
using Freem.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class EventsRepositoryTests : BaseRepositoryTests<IEventsRepository>
{
    public EventsRepositoryTests(
        ITestOutputHelper output) 
        : base(output)
    {
    }

    public delegate IEntityEvent<ActivityIdentifier, UserIdentifier> ActivityEventGenerator(
        EventIdentifier id,
        ActivityIdentifier activityId, 
        UserIdentifier userId);

    public delegate IEntityEvent<RecordIdentifier, UserIdentifier> RecordEventGenerator(
        EventIdentifier id,
        RecordIdentifier recordId,
        UserIdentifier userId);

    public delegate IEntityEvent<RunningRecordIdentifier, UserIdentifier> RunningRecordEventGenerator(
        EventIdentifier id,
        RunningRecordIdentifier runningRecordId);

    public delegate IEntityEvent<TagIdentifier, UserIdentifier> TagEventGenerator(
        EventIdentifier id,
        TagIdentifier tagId,
        UserIdentifier userId);
    
    public delegate IEntityEvent<UserIdentifier, UserIdentifier> UserEventGenerator(
        EventIdentifier id,
        UserIdentifier userId);
    
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

        var id = IdentifiersGenerator.Generate<EventIdentifier>();
        var userId = (UserIdentifier)dbUser.Id;
        var entityId = (ActivityIdentifier)dbActivity.Id;
        
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
        
        var id = IdentifiersGenerator.Generate<EventIdentifier>();
        var userId = (UserIdentifier)dbUser.Id;
        var entityId = (RecordIdentifier)dbRecord.Id;
        
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
        
        var id = IdentifiersGenerator.Generate<EventIdentifier>();
        var entityId = (RunningRecordIdentifier)dbRecord.UserId;

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
        Assert.Equal(entityId, result.Entity.EntityId);
        Assert.Equal(entityId, result.Entity.UserEntityId);
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
        
        var id = IdentifiersGenerator.Generate<EventIdentifier>();
        var entityId = (TagIdentifier)dbTag.Id;
        var userId = (UserIdentifier)dbUser.Id;
        
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
            (id, userId) => new UserSignedInEvent(id, userId),
            (id, userId) => new UserSettingsChanged(id, userId),
            (id, userId) => new UserPasswordCredentialsChangedEvent(id, userId),
            (id, userId) => new UserTelegramIntegrationChangedEvent(id, userId),
        };

    [Theory]
    [MemberData(nameof(ValidUserEventsCase))]
    public async Task CreateAsync_ShouldSuccess_WhenPassUserEvent(UserEventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        
        await Database.Users.AddAsync(dbUser);
        await Database.SaveChangesAsync();
        
        var id = IdentifiersGenerator.Generate<EventIdentifier>();
        var userId = (UserIdentifier)dbUser.Id;
        
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
        
        var id = IdentifiersGenerator.Generate<EventIdentifier>();
        var userId = IdentifiersGenerator.Generate<UserIdentifier>();

        var activityId = IdentifiersGenerator.Generate<ActivityIdentifier>();
        foreach (var @case in ValidActivityEntitiesCases)
        {
            var generator = (ActivityEventGenerator)@case[0];
            var @event = generator(id, activityId, userId);
            
            types.Add(@event.GetType());
        }
        
        var recordId = IdentifiersGenerator.Generate<RecordIdentifier>();
        foreach (var @case in ValidRecordEntitiesCases)
        {
            var generator = (RecordEventGenerator)@case[0];
            var @event = generator(id, recordId, userId);
            
            types.Add(@event.GetType());
        }

        var runningRecordId = IdentifiersGenerator.Generate<RunningRecordIdentifier>();
        foreach (var @case in ValidRunningRecordEventsCases)
        {
            var generator = (RunningRecordEventGenerator)@case[0];
            var @event = generator(id, runningRecordId);
            
            types.Add(@event.GetType());
        }
        
        var tagId = IdentifiersGenerator.Generate<TagIdentifier>();
        foreach (var @case in ValidTagEventsCase)
        {
            var generator = (TagEventGenerator)@case[0];
            var @event = generator(id, tagId, userId);
            
            types.Add(@event.GetType());
        }

        foreach (var @case in ValidUserEventsCase)
        {
            var generator = (UserEventGenerator)@case[0];
            var @event = generator(id, userId);
            
            types.Add(@event.GetType());
        }

        var eventTypes = TypeLoader.GetImplementations("Freem.Entities", "IEntityEvent`2");

        // Act
        var result = types.UnorderedEquals(eventTypes, EqualityComparer<Type>.Default);
        
        // Assert
        Assert.True(result);
    }
}
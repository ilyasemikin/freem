using Freem.Domain.Activities;
using Freem.Domain.Activities.Events;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.RunningRecords;
using Freem.Domain.RunningRecords.Events;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Storage.Serialization;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Events;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Events;
using Freem.Domain.Users.Models;
using Freem.Time;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class EventsRepositoryTests : BaseRepositoryTests<IEventRepository>
{
    private static readonly DateTimeOffset OccurredAt = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    public EventsRepositoryTests(ITestOutputHelper output)
        : base(output)
    {
    }

    public delegate EntityEvent EventGenerator(EventId id, string entityId, IReadOnlyList<ActivityId> activityIds, IReadOnlyList<TagId> tagIds);

    public static TheoryData<EntityType, EventGenerator> ValidEventsCases
    {
        get
        {
            var activityName = ActivityName.Parse("activity", null);
            var recordName = RecordName.Parse("record", null);
            var tagName = TagName.Parse("tag", null);
            var nickname = Nickname.Parse("nickname", null);
            var offset = new TimeZoneOffset(TimeSpan.FromHours(3));
            var period = new DateTimePeriod(OccurredAt, TimeSpan.FromHours(1));
            var periods = new[] { new RecordPeriodSnapshot(RecordPeriodId.Generate(), period, offset) };

            return new TheoryData<EntityType, EventGenerator>
            {
                { EntityType.Activity, (id, e, _, tags) => new ActivityCreatedEvent(id) { ActivityId = ActivityId.Parse(e, null), Name = activityName, Status = ActivityStatus.Active, TagIds = tags } },
                { EntityType.Activity, (id, e, _, tags) => new ActivityUpdatedEvent(id) { ActivityId = ActivityId.Parse(e, null), Name = activityName, Status = ActivityStatus.Active, TagIds = tags } },
                { EntityType.Activity, (id, e, _, _) => new ActivityRemovedEvent(id) { ActivityId = ActivityId.Parse(e, null) } },
                { EntityType.Activity, (id, e, _, _) => new ActivityArchivedEvent(id) { ActivityId = ActivityId.Parse(e, null) } },
                { EntityType.Activity, (id, e, _, _) => new ActivityUnarchivedEvent(id) { ActivityId = ActivityId.Parse(e, null) } },
                { EntityType.Record, (id, e, activities, tags) => new RecordCreatedEvent(id) { RecordId = RecordId.Parse(e, null), Name = recordName, Periods = periods, ActivityIds = activities, TagIds = tags } },
                { EntityType.Record, (id, e, activities, tags) => new RecordUpdatedEvent(id) { RecordId = RecordId.Parse(e, null), Periods = periods, ActivityIds = activities, TagIds = tags } },
                { EntityType.Record, (id, e, _, _) => new RecordRemovedEvent(id) { RecordId = RecordId.Parse(e, null) } },
                { EntityType.RunningRecord, (id, e, activities, tags) => new RunningRecordStartedEvent(id) { RunningRecordId = RunningRecordId.Parse(e, null), StartAt = OccurredAt, ActivityIds = activities, TagIds = tags } },
                { EntityType.RunningRecord, (id, e, activities, tags) => new RunningRecordUpdatedEvent(id) { RunningRecordId = RunningRecordId.Parse(e, null), StartAt = OccurredAt, ActivityIds = activities, TagIds = tags } },
                { EntityType.RunningRecord, (id, e, _, _) => new RunningRecordStoppedEvent(id) { RunningRecordId = RunningRecordId.Parse(e, null) } },
                { EntityType.RunningRecord, (id, e, _, _) => new RunningRecordRemovedEvent(id) { RunningRecordId = RunningRecordId.Parse(e, null) } },
                { EntityType.Tag, (id, e, _, _) => new TagCreatedEvent(id) { TagId = TagId.Parse(e, null), Name = tagName } },
                { EntityType.Tag, (id, e, _, _) => new TagUpdatedEvent(id) { TagId = TagId.Parse(e, null), Name = tagName } },
                { EntityType.Tag, (id, e, _, _) => new TagRemovedEvent(id) { TagId = TagId.Parse(e, null) } },
                { EntityType.User, (id, e, _, _) => new UserRegisteredEvent(id) { UserId = UserId.Parse(e, null), Nickname = nickname } },
                { EntityType.User, (id, e, _, _) => new UserSettingsChangedEvent(id) { UserId = UserId.Parse(e, null), TimeZoneOffset = offset } },
            };
        }
    }

    [Theory]
    [MemberData(nameof(ValidEventsCases))]
    public async Task CreateAsync_ShouldSuccess_WhenPassEvent(EntityType entityType, EventGenerator factory)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        var dbTag = EntitiesFactory.CreateTag();
        var dbRecord = EntitiesFactory.CreateRecord();
        var dbRunningRecord = EntitiesFactory.CreateRunningRecord();

        dbRecord.Activities = [dbActivity];
        dbRunningRecord.Activities = [dbActivity];

        await Database.AddRangeAsync(dbUser, dbActivity, dbTag);
        await Database.AddRangeAsync(dbRecord, dbRunningRecord);
        await Database.SaveChangesAsync();

        var entityId = entityType switch
        {
            EntityType.Activity => dbActivity.Id,
            EntityType.Record => dbRecord.Id,
            EntityType.RunningRecord => dbRunningRecord.UserId,
            EntityType.Tag => dbTag.Id,
            EntityType.User => dbUser.Id,
            _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
        };

        var userId = UserId.Parse(dbUser.Id, null);
        var activityIds = new[] { ActivityId.Parse(dbActivity.Id, null) };
        var tagIds = new[] { TagId.Parse(dbTag.Id, null) };

        var id = EventId.Generate();
        var @event = factory(id, entityId, activityIds, tagIds);
        var envelope = new EventEnvelope(userId, OccurredAt, @event);

        // Act
        var writeResult = await Repository.CreateAsync(envelope);
        var result = await Repository.FindByIdAsync(id);

        // Assert
        Assert.True(writeResult.Success);
        Assert.True(result.Success);

        var stored = result.Value;
        Assert.Equal(id, stored.Id);
        Assert.Equal(@event.Type, stored.Type);
        Assert.Equal(userId, stored.UserId);
        Assert.Equal(OccurredAt, stored.OccurredAt);
        Assert.Equal(@event.EntityRef, stored.EntityRef);

        var deserialized = EventPayloadSerializer.Deserialize(stored);
        Assert.True(deserialized.Success);
        Assert.Equal(@event.GetType(), deserialized.Value.Event.GetType());

        var expectedPayload = EventPayloadSerializer.Serialize(@event).ToReadOnlyDictionary();
        Assert.Equal(expectedPayload, stored.Payload.ToReadOnlyDictionary());
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnNotFound_WhenEventNotExists()
    {
        var id = EventId.Generate();

        var result = await Repository.FindByIdAsync(id);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnUserEventsSortedByOccurredAtDescending()
    {
        // Arrange
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();
        var firstTag = firstFactory.CreateTag();
        var secondTag = secondFactory.CreateTag();

        await Database.AddRangeAsync(firstFactory.User, secondFactory.User, firstTag, secondTag);
        await Database.SaveChangesAsync();

        var firstUserId = UserId.Parse(firstFactory.User.Id, null);
        var secondUserId = UserId.Parse(secondFactory.User.Id, null);

        var older = await CreateTagEventAsync(firstUserId, firstTag.Id, OccurredAt);
        var newer = await CreateTagEventAsync(firstUserId, firstTag.Id, OccurredAt.AddHours(1));
        await CreateTagEventAsync(secondUserId, secondTag.Id, OccurredAt.AddHours(2));

        var filter = new EventByUserFilter(firstUserId) { Limit = 100, Offset = 0 };

        // Act
        var result = await Repository.FindAsync(filter);

        // Assert
        Assert.True(result.Success);
        var events = await result.Value.ToArrayAsync();
        Assert.Equal(2, result.Value.Count);
        Assert.Equal([newer, older], events.Select(e => e.Id));
    }

    [Fact]
    public async Task FindAsync_ShouldFilterByEntityTypeEntityRefAndPeriod()
    {
        // Arrange
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var firstTag = factory.CreateTag();
        var secondTag = factory.CreateTag();

        await Database.AddRangeAsync(factory.User, firstTag, secondTag);
        await Database.SaveChangesAsync();

        var userId = UserId.Parse(factory.User.Id, null);

        var first = await CreateTagEventAsync(userId, firstTag.Id, OccurredAt);
        await CreateTagEventAsync(userId, secondTag.Id, OccurredAt.AddHours(1));
        var last = await CreateTagEventAsync(userId, firstTag.Id, OccurredAt.AddHours(2));

        // Act
        var byEntityType = await Repository.FindAsync(new EventByUserFilter(userId)
        {
            Limit = 100, Offset = 0, EntityType = EntityType.Activity
        });
        var byEntityRef = await Repository.FindAsync(new EventByUserFilter(userId)
        {
            Limit = 100, Offset = 0, EntityType = EntityType.Tag, EntityRef = firstTag.Id
        });
        var byPeriod = await Repository.FindAsync(new EventByUserFilter(userId)
        {
            Limit = 100, Offset = 0, From = OccurredAt.AddMinutes(30), To = OccurredAt.AddHours(2)
        });

        // Assert
        Assert.Equal(0, byEntityType.Value.Count);

        var byEntityRefEvents = await byEntityRef.Value.ToArrayAsync();
        Assert.Equal([last, first], byEntityRefEvents.Select(e => e.Id));

        Assert.Equal(2, byPeriod.Value.Count);
    }

    [Fact]
    public async Task FindAsync_ShouldApplyLimitAndOffset_AndReturnTotalCount()
    {
        // Arrange
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var tag = factory.CreateTag();

        await Database.AddRangeAsync(factory.User, tag);
        await Database.SaveChangesAsync();

        var userId = UserId.Parse(factory.User.Id, null);

        await CreateTagEventAsync(userId, tag.Id, OccurredAt);
        var middle = await CreateTagEventAsync(userId, tag.Id, OccurredAt.AddHours(1));
        await CreateTagEventAsync(userId, tag.Id, OccurredAt.AddHours(2));

        // Act
        var result = await Repository.FindAsync(new EventByUserFilter(userId) { Limit = 1, Offset = 1 });

        // Assert
        Assert.True(result.Success);
        Assert.Equal(3, result.Value.Count);
        var events = await result.Value.ToArrayAsync();
        Assert.Equal([middle], events.Select(e => e.Id));
    }

    [Fact]
    public async Task CreateManyAsync_ShouldCreateAllEvents()
    {
        // Arrange
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var tag = factory.CreateTag();

        await Database.AddRangeAsync(factory.User, tag);
        await Database.SaveChangesAsync();

        var userId = UserId.Parse(factory.User.Id, null);
        var tagId = TagId.Parse(tag.Id, null);
        var tagName = TagName.Parse("tag", null);

        var envelopes = Enumerable
            .Range(0, 3)
            .Select(_ =>
            {
                var @event = new TagUpdatedEvent(EventId.Generate()) { TagId = tagId, Name = tagName };
                return new EventEnvelope(userId, OccurredAt, @event);
            })
            .ToArray();

        // Act
        var writeResult = await Repository.CreateManyAsync(envelopes);

        // Assert
        Assert.True(writeResult.Success);

        var result = await Repository.FindAsync(new EventByUserFilter(userId) { Limit = 100, Offset = 0 });
        Assert.True(result.Success);
        var events = await result.Value.ToArrayAsync();
        var expected = envelopes.Select(e => e.Event.Id.ToString()).Order();
        var actual = events.Select(e => e.Id.ToString()).Order();
        Assert.Equal(expected, actual);
    }

    private async Task<EventId> CreateTagEventAsync(UserId userId, string tagId, DateTimeOffset occurredAt)
    {
        var id = EventId.Generate();
        var @event = new TagUpdatedEvent(id)
        {
            TagId = TagId.Parse(tagId, null),
            Name = TagName.Parse("tag", null)
        };

        var envelope = new EventEnvelope(userId, occurredAt, @event);
        var result = await Repository.CreateAsync(envelope);
        Assert.True(result.Success);

        return id;
    }
}

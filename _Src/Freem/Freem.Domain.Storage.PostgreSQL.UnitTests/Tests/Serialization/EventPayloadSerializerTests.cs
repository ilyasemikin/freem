using Freem.Domain.Activities;
using Freem.Domain.Activities.Events;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Events;
using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.RunningRecords;
using Freem.Domain.RunningRecords.Events;
using Freem.Domain.Storage.Serialization;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Events;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Events;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Domain.Storage.PostgreSQL.UnitTests.Tests.Serialization;

public sealed class EventPayloadSerializerTests
{
    private static readonly UserId UserId = UserId.Generate();
    private static readonly ActivityId ActivityId = ActivityId.Generate();
    private static readonly TagId FirstTagId = TagId.Generate();
    private static readonly TagId SecondTagId = TagId.Generate();
    private static readonly RecordId RecordId = RecordId.Generate();
    private static readonly RunningRecordId RunningRecordId = RunningRecordId.FromUserId(UserId);
    private static readonly TagId[] TagIds = [FirstTagId, SecondTagId];
    private static readonly ActivityId[] ActivityIds = [ActivityId];
    private static readonly DateTimeOffset StartAt = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
    private static readonly TimeZoneOffset PeriodOffset = new(TimeSpan.FromMinutes(-330));
    private static readonly RecordPeriodSnapshot[] Periods =
    [
        new(RecordPeriodId.Generate(), new DateTimePeriod(StartAt, TimeSpan.FromHours(1)), PeriodOffset, RecordPeriodDescription.Parse("first, \"quoted\"\nline", null)),
        new(RecordPeriodId.Generate(), new DateTimePeriod(StartAt.AddHours(2), TimeSpan.FromHours(1)), TimeZoneOffset.Default)
    ];

    public static TheoryData<EntityEvent> Events
    {
        get
        {
            var id = EventId.Generate();
            var activityName = ActivityName.Parse("activity", null);
            var activityColor = ActivityColor.Parse("#6366f1", null);
            var recordName = RecordName.Parse("record", null);
            var recordDescription = RecordDescription.Parse("description", null);
            var tagName = TagName.Parse("tag", null);
            var nickname = Nickname.Parse("nickname", null);
            var offset = new TimeZoneOffset(TimeSpan.FromHours(3));

            return new TheoryData<EntityEvent>
            {
                new ActivityCreatedEvent(id) { ActivityId = ActivityId, Name = activityName, Color = activityColor, Status = ActivityStatus.Active, TagIds = TagIds },
                new ActivityUpdatedEvent(id) { ActivityId = ActivityId, Name = activityName, Color = activityColor, Status = ActivityStatus.Archived, TagIds = [] },
                new ActivityRemovedEvent(id) { ActivityId = ActivityId },
                new ActivityArchivedEvent(id) { ActivityId = ActivityId },
                new ActivityUnarchivedEvent(id) { ActivityId = ActivityId },
                new RecordCreatedEvent(id) { RecordId = RecordId, Name = recordName, Description = recordDescription, Periods = Periods, ActivityIds = ActivityIds, TagIds = TagIds },
                new RecordUpdatedEvent(id) { RecordId = RecordId, Periods = Periods, ActivityIds = ActivityIds, TagIds = [] },
                new RecordRemovedEvent(id) { RecordId = RecordId },
                new RunningRecordStartedEvent(id) { RunningRecordId = RunningRecordId, Name = recordName, StartAt = StartAt, ActivityIds = ActivityIds, TagIds = TagIds },
                new RunningRecordUpdatedEvent(id) { RunningRecordId = RunningRecordId, Description = recordDescription, StartAt = StartAt, ActivityIds = ActivityIds, TagIds = [] },
                new RunningRecordStoppedEvent(id) { RunningRecordId = RunningRecordId },
                new RunningRecordRemovedEvent(id) { RunningRecordId = RunningRecordId },
                new TagCreatedEvent(id) { TagId = FirstTagId, Name = tagName },
                new TagUpdatedEvent(id) { TagId = FirstTagId, Name = tagName },
                new TagRemovedEvent(id) { TagId = FirstTagId },
                new UserRegisteredEvent(id) { UserId = UserId, Nickname = nickname },
                new UserSettingsChangedEvent(id) { UserId = UserId, TimeZoneOffset = offset }
            };
        }
    }

    [Theory]
    [MemberData(nameof(Events))]
    public void Deserialize_ShouldRestoreEvent_AfterSerialize(EntityEvent @event)
    {
        var record = CreateRecord(@event);

        var result = EventPayloadSerializer.Deserialize(record);

        Assert.True(result.Success);
        var restored = result.Value.Event;
        Assert.Equal(@event.GetType(), restored.GetType());
        Assert.Equal(@event.Id, restored.Id);
        Assert.Equal(@event.Type, restored.Type);
        Assert.Equal(@event.EntityRef, restored.EntityRef);
        Assert.Equal(UserId, result.Value.UserId);
        Assert.Equal(record.OccurredAt, result.Value.OccurredAt);

        var expectedPayload = EventPayloadSerializer.Serialize(@event).ToReadOnlyDictionary();
        var actualPayload = EventPayloadSerializer.Serialize(restored).ToReadOnlyDictionary();
        Assert.Equal(expectedPayload, actualPayload);
    }

    [Fact]
    public void Deserialize_ShouldRestoreSnapshot_WhenEventIsRecordCreated()
    {
        var recordName = RecordName.Parse("record", null);
        var @event = new RecordCreatedEvent(EventId.Generate())
        {
            RecordId = RecordId,
            Name = recordName,
            Periods = Periods,
            ActivityIds = ActivityIds,
            TagIds = TagIds
        };

        var result = EventPayloadSerializer.Deserialize(CreateRecord(@event));

        var restored = Assert.IsType<RecordCreatedEvent>(result.Value.Event);
        Assert.Equal(recordName, restored.Name);
        Assert.Null(restored.Description);
        Assert.Equal(Periods.Length, restored.Periods.Count);
        for (var i = 0; i < Periods.Length; i++)
        {
            Assert.Equal(Periods[i].Id, restored.Periods[i].Id);
            Assert.Equal(Periods[i].Period, restored.Periods[i].Period);
            Assert.Equal(Periods[i].TimeZoneOffset, restored.Periods[i].TimeZoneOffset);
            Assert.Equal(Periods[i].Description, restored.Periods[i].Description);
        }
        Assert.Equal(ActivityIds, restored.ActivityIds);
        Assert.Equal(TagIds, restored.TagIds);
    }

    [Fact]
    public void Deserialize_ShouldFail_WhenEventTypeIsUnknown()
    {
        var record = CreateRecord(new TagRemovedEvent(EventId.Generate()) { TagId = FirstTagId }, "unknown.type");

        var result = EventPayloadSerializer.Deserialize(record);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EventDeserializationFailed, result.Error.Type);
    }

    [Fact]
    public void Deserialize_ShouldFail_WhenRequiredPayloadKeyIsMissing()
    {
        var @event = new TagCreatedEvent(EventId.Generate()) { TagId = FirstTagId, Name = TagName.Parse("tag", null) };
        var record = CreateRecord(@event, payload: new EventPayload(Utilities.Naming.Abstractions.INamingPolicy.CamelCase));

        var result = EventPayloadSerializer.Deserialize(record);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EventDeserializationFailed, result.Error.Type);
    }

    [Theory]
    [InlineData("not a json")]
    [InlineData("null")]
    [InlineData("[{\"id\":\"p\",\"startAt\":\"2026-10-20T11:00:00+00:00\",\"endAt\":\"2026-10-20T10:00:00+00:00\"}]")]
    public void Deserialize_ShouldFail_WhenPeriodsPayloadIsInvalid(string periods)
    {
        var @event = new RecordCreatedEvent(EventId.Generate())
        {
            RecordId = RecordId,
            Periods = Periods,
            ActivityIds = ActivityIds,
            TagIds = TagIds
        };
        var payload = EventPayloadSerializer.Serialize(@event).Set(nameof(RecordCreatedEvent.Periods), periods);

        var result = EventPayloadSerializer.Deserialize(CreateRecord(@event, payload: payload));

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EventDeserializationFailed, result.Error.Type);
    }

    [Fact]
    public void Deserialize_ShouldUseDefaultOffset_WhenPeriodPayloadHasNoOffset()
    {
        var @event = new RecordCreatedEvent(EventId.Generate())
        {
            RecordId = RecordId,
            Periods = Periods,
            ActivityIds = ActivityIds,
            TagIds = TagIds
        };
        var periodId = Periods[0].Id.ToString();
        var periods = $$"""[{"Id":"{{periodId}}","StartAt":"2026-10-20T10:00:00+00:00","EndAt":"2026-10-20T11:00:00+00:00","Description":null}]""";
        var payload = EventPayloadSerializer.Serialize(@event).Set(nameof(RecordCreatedEvent.Periods), periods);

        var result = EventPayloadSerializer.Deserialize(CreateRecord(@event, payload: payload));

        Assert.True(result.Success);
        var restored = Assert.IsType<RecordCreatedEvent>(result.Value.Event);
        var period = Assert.Single(restored.Periods);
        Assert.Equal(TimeZoneOffset.Default, period.TimeZoneOffset);
    }

    [Fact]
    public void Deserialize_ShouldUseDefaultOffset_WhenSettingsPayloadHasNoOffset()
    {
        var offset = new TimeZoneOffset(TimeSpan.FromHours(3));
        var @event = new UserSettingsChangedEvent(EventId.Generate()) { UserId = UserId, TimeZoneOffset = offset };
        var payload = new EventPayload(Utilities.Naming.Abstractions.INamingPolicy.CamelCase);

        var result = EventPayloadSerializer.Deserialize(CreateRecord(@event, payload: payload));

        Assert.True(result.Success);
        var restored = Assert.IsType<UserSettingsChangedEvent>(result.Value.Event);
        Assert.Equal(TimeZoneOffset.Default, restored.TimeZoneOffset);
    }

    [Fact]
    public void Events_ShouldAllProcessed()
    {
        IEnumerable<object[]> rows = Events;
        var covered = rows
            .Select(row => row[0].GetType())
            .ToHashSet();

        var expected = typeof(EntityEvent).Assembly.ExportedTypes
            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(EntityEvent)))
            .ToHashSet();

        Assert.True(expected.SetEquals(covered));
    }

    private static Event CreateRecord(EntityEvent @event, string? type = null, EventPayload? payload = null)
    {
        var eventType = type is null ? @event.Type : EventType.Parse(type, null);
        return new Event(@event.Id)
        {
            Type = eventType,
            UserId = UserId,
            OccurredAt = StartAt,
            EntityRef = @event.EntityRef,
            Payload = payload ?? EventPayloadSerializer.Serialize(@event)
        };
    }
}

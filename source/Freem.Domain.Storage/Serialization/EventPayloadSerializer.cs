using System.Text.Json;
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
using Freem.Domain.Tags;
using Freem.Domain.Tags.Events;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Events;
using Freem.Domain.Users.Models;
using Freem.Time;
using Utilities.Errors;
using Utilities.Naming.Abstractions;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Serialization;

public static class EventPayloadSerializer
{
    private sealed record RecordPeriodPayload(
        string Id,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        string? Description,
        string? TimeZoneOffset);

    private static readonly INamingPolicy Policy = INamingPolicy.CamelCase;

    public static EventPayload Serialize(EntityEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        switch (@event)
        {
            case ActivityCreatedEvent e:
                return SerializeActivity(e.Name, e.Color, e.Status, e.TagIds);
            case ActivityUpdatedEvent e:
                return SerializeActivity(e.Name, e.Color, e.Status, e.TagIds);
            case ActivityRemovedEvent:
            case ActivityArchivedEvent:
            case ActivityUnarchivedEvent:
                return new EventPayload(Policy);
            case RecordCreatedEvent e:
                return SerializeRecord(e.Name, e.Description, e.Periods, e.ActivityIds, e.TagIds);
            case RecordUpdatedEvent e:
                return SerializeRecord(e.Name, e.Description, e.Periods, e.ActivityIds, e.TagIds);
            case RecordRemovedEvent:
                return new EventPayload(Policy);
            case RunningRecordStartedEvent e:
                return SerializeRunningRecord(e.Name, e.Description, e.StartAt, e.ActivityIds, e.TagIds);
            case RunningRecordUpdatedEvent e:
                return SerializeRunningRecord(e.Name, e.Description, e.StartAt, e.ActivityIds, e.TagIds);
            case RunningRecordStoppedEvent:
            case RunningRecordRemovedEvent:
                return new EventPayload(Policy);
            case TagCreatedEvent e:
                return SerializeTag(e.Name);
            case TagUpdatedEvent e:
                return SerializeTag(e.Name);
            case TagRemovedEvent:
                return new EventPayload(Policy);
            case UserRegisteredEvent e:
                return new EventPayload(Policy)
                    .Set(nameof(UserRegisteredEvent.Nickname), e.Nickname);
            case UserSettingsChangedEvent e:
                return new EventPayload(Policy)
                    .Set(nameof(UserSettingsChangedEvent.TimeZoneOffset), e.TimeZoneOffset)
                    .Set(nameof(UserSettingsChangedEvent.WeekStart), e.WeekStart);
            default:
                throw new InvalidOperationException($"Unmapped event type [{@event.Type}]");
        }
    }

    public static Result<EventEnvelope, Error> Deserialize(Event record)
    {
        ArgumentNullException.ThrowIfNull(record);

        var type = record.Type.ToString();

        try
        {
            var @event = DeserializeEvent(record);
            if (@event is null)
                return DomainErrorCatalog.Factories.CreateEventDeserializationFailed(type, "unknown event type");

            return new EventEnvelope(record.UserId, record.OccurredAt, @event);
        }
        catch (Exception exception)
        {
            return DomainErrorCatalog.Factories.CreateEventDeserializationFailed(type, exception.Message);
        }
    }

    private static EntityEvent? DeserializeEvent(Event record)
    {
        var type = record.Type;
        var payload = record.Payload;
        var entityRef = record.EntityRef.Value;

        if (type.Equals(EventTypes.Activities.Created))
            return new ActivityCreatedEvent(record.Id)
            {
                ActivityId = ActivityId.Parse(entityRef, null),
                Name = payload.Get(nameof(ActivityCreatedEvent.Name), ActivityName.Parse),
                Color = payload.GetOptional(nameof(ActivityCreatedEvent.Color), ActivityColor.Parse),
                Status = payload.Get(nameof(ActivityCreatedEvent.Status), ActivityStatus.Parse),
                TagIds = payload.GetList(nameof(ActivityCreatedEvent.TagIds), TagId.Parse)
            };
        if (type.Equals(EventTypes.Activities.Updated))
            return new ActivityUpdatedEvent(record.Id)
            {
                ActivityId = ActivityId.Parse(entityRef, null),
                Name = payload.Get(nameof(ActivityUpdatedEvent.Name), ActivityName.Parse),
                Color = payload.GetOptional(nameof(ActivityUpdatedEvent.Color), ActivityColor.Parse),
                Status = payload.Get(nameof(ActivityUpdatedEvent.Status), ActivityStatus.Parse),
                TagIds = payload.GetList(nameof(ActivityUpdatedEvent.TagIds), TagId.Parse)
            };
        if (type.Equals(EventTypes.Activities.Removed))
            return new ActivityRemovedEvent(record.Id)
            {
                ActivityId = ActivityId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.Activities.Archived))
            return new ActivityArchivedEvent(record.Id)
            {
                ActivityId = ActivityId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.Activities.Unarchived))
            return new ActivityUnarchivedEvent(record.Id)
            {
                ActivityId = ActivityId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.Records.Created))
            return new RecordCreatedEvent(record.Id)
            {
                RecordId = RecordId.Parse(entityRef, null),
                Name = payload.GetOptional(nameof(RecordCreatedEvent.Name), RecordName.Parse),
                Description = payload.GetOptional(nameof(RecordCreatedEvent.Description), RecordDescription.Parse),
                Periods = DeserializePeriods(payload),
                ActivityIds = payload.GetList(nameof(RecordCreatedEvent.ActivityIds), ActivityId.Parse),
                TagIds = payload.GetList(nameof(RecordCreatedEvent.TagIds), TagId.Parse)
            };
        if (type.Equals(EventTypes.Records.Updated))
            return new RecordUpdatedEvent(record.Id)
            {
                RecordId = RecordId.Parse(entityRef, null),
                Name = payload.GetOptional(nameof(RecordUpdatedEvent.Name), RecordName.Parse),
                Description = payload.GetOptional(nameof(RecordUpdatedEvent.Description), RecordDescription.Parse),
                Periods = DeserializePeriods(payload),
                ActivityIds = payload.GetList(nameof(RecordUpdatedEvent.ActivityIds), ActivityId.Parse),
                TagIds = payload.GetList(nameof(RecordUpdatedEvent.TagIds), TagId.Parse)
            };
        if (type.Equals(EventTypes.Records.Removed))
            return new RecordRemovedEvent(record.Id)
            {
                RecordId = RecordId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.RunningRecords.Started))
            return new RunningRecordStartedEvent(record.Id)
            {
                RunningRecordId = RunningRecordId.Parse(entityRef, null),
                Name = payload.GetOptional(nameof(RunningRecordStartedEvent.Name), RecordName.Parse),
                Description = payload.GetOptional(nameof(RunningRecordStartedEvent.Description), RecordDescription.Parse),
                StartAt = payload.GetDate(nameof(RunningRecordStartedEvent.StartAt)),
                ActivityIds = payload.GetList(nameof(RunningRecordStartedEvent.ActivityIds), ActivityId.Parse),
                TagIds = payload.GetList(nameof(RunningRecordStartedEvent.TagIds), TagId.Parse)
            };
        if (type.Equals(EventTypes.RunningRecords.Updated))
            return new RunningRecordUpdatedEvent(record.Id)
            {
                RunningRecordId = RunningRecordId.Parse(entityRef, null),
                Name = payload.GetOptional(nameof(RunningRecordUpdatedEvent.Name), RecordName.Parse),
                Description = payload.GetOptional(nameof(RunningRecordUpdatedEvent.Description), RecordDescription.Parse),
                StartAt = payload.GetDate(nameof(RunningRecordUpdatedEvent.StartAt)),
                ActivityIds = payload.GetList(nameof(RunningRecordUpdatedEvent.ActivityIds), ActivityId.Parse),
                TagIds = payload.GetList(nameof(RunningRecordUpdatedEvent.TagIds), TagId.Parse)
            };
        if (type.Equals(EventTypes.RunningRecords.Stopped))
            return new RunningRecordStoppedEvent(record.Id)
            {
                RunningRecordId = RunningRecordId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.RunningRecords.Removed))
            return new RunningRecordRemovedEvent(record.Id)
            {
                RunningRecordId = RunningRecordId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.Tags.Created))
            return new TagCreatedEvent(record.Id)
            {
                TagId = TagId.Parse(entityRef, null),
                Name = payload.Get(nameof(TagCreatedEvent.Name), TagName.Parse)
            };
        if (type.Equals(EventTypes.Tags.Updated))
            return new TagUpdatedEvent(record.Id)
            {
                TagId = TagId.Parse(entityRef, null),
                Name = payload.Get(nameof(TagUpdatedEvent.Name), TagName.Parse)
            };
        if (type.Equals(EventTypes.Tags.Removed))
            return new TagRemovedEvent(record.Id)
            {
                TagId = TagId.Parse(entityRef, null)
            };
        if (type.Equals(EventTypes.Users.Registered))
            return new UserRegisteredEvent(record.Id)
            {
                UserId = UserId.Parse(entityRef, null),
                Nickname = payload.Get(nameof(UserRegisteredEvent.Nickname), Nickname.Parse)
            };
        if (type.Equals(EventTypes.Users.SettingsChanged))
            return new UserSettingsChangedEvent(record.Id)
            {
                UserId = UserId.Parse(entityRef, null),
                TimeZoneOffset = payload.GetOptional(nameof(UserSettingsChangedEvent.TimeZoneOffset), TimeZoneOffset.Parse) ?? TimeZoneOffset.Default,
                WeekStart = payload.GetOptional(nameof(UserSettingsChangedEvent.WeekStart), WeekStart.Parse) ?? WeekStart.Default
            };

        return null;
    }

    private static IReadOnlyList<RecordPeriodSnapshot> DeserializePeriods(EventPayload payload)
    {
        var json = payload.GetRequired(nameof(RecordCreatedEvent.Periods));
        var items = DeserializePeriodPayloads(json);

        return items.Select(MapToSnapshot).ToArray();
    }

    private static RecordPeriodPayload[] DeserializePeriodPayloads(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<RecordPeriodPayload[]>(json) ??
                   throw new FormatException("Invalid value for record periods");
        }
        catch (JsonException exception)
        {
            throw new FormatException("Invalid value for record periods", exception);
        }
    }

    private static RecordPeriodSnapshot MapToSnapshot(RecordPeriodPayload item)
    {
        var id = RecordPeriodId.Parse(item.Id, null);
        if (!DateTimePeriod.TryCreate(item.StartAt, item.EndAt, out var period))
            throw new FormatException($"Invalid value for record period: {item.StartAt:O},{item.EndAt:O}");

        var description = item.Description is null
            ? null
            : RecordPeriodDescription.Parse(item.Description, null);
        // Payloads written before the offset was stored have no value
        var timeZoneOffset = item.TimeZoneOffset is null
            ? TimeZoneOffset.Default
            : TimeZoneOffset.Parse(item.TimeZoneOffset, null);
        return new RecordPeriodSnapshot(id, period, timeZoneOffset, description);
    }

    private static RecordPeriodPayload MapToPayload(RecordPeriodSnapshot snapshot)
    {
        return new RecordPeriodPayload(
            snapshot.Id.ToString(),
            snapshot.Period.StartAt,
            snapshot.Period.EndAt,
            snapshot.Description?.ToString(),
            snapshot.TimeZoneOffset.ToString());
    }

    private static EventPayload SerializeActivity(
        ActivityName name, ActivityColor? color, ActivityStatus status, IEnumerable<TagId> tagIds)
    {
        return new EventPayload(Policy)
            .Set(nameof(ActivityCreatedEvent.Name), name)
            .Set(nameof(ActivityCreatedEvent.Color), color)
            .Set(nameof(ActivityCreatedEvent.Status), status)
            .SetList(nameof(ActivityCreatedEvent.TagIds), tagIds);
    }

    private static EventPayload SerializeRecord(
        RecordName? name,
        RecordDescription? description,
        IEnumerable<RecordPeriodSnapshot> periods,
        IEnumerable<ActivityId> activityIds,
        IEnumerable<TagId> tagIds)
    {
        var payloads = periods.Select(MapToPayload).ToArray();
        var periodsJson = JsonSerializer.Serialize(payloads);
        return new EventPayload(Policy)
            .Set(nameof(RecordCreatedEvent.Name), name)
            .Set(nameof(RecordCreatedEvent.Description), description)
            .Set(nameof(RecordCreatedEvent.Periods), periodsJson)
            .SetList(nameof(RecordCreatedEvent.ActivityIds), activityIds)
            .SetList(nameof(RecordCreatedEvent.TagIds), tagIds);
    }

    private static EventPayload SerializeRunningRecord(
        RecordName? name,
        RecordDescription? description,
        DateTimeOffset startAt,
        IEnumerable<ActivityId> activityIds,
        IEnumerable<TagId> tagIds)
    {
        return new EventPayload(Policy)
            .Set(nameof(RunningRecordStartedEvent.Name), name)
            .Set(nameof(RunningRecordStartedEvent.Description), description)
            .SetDate(nameof(RunningRecordStartedEvent.StartAt), startAt)
            .SetList(nameof(RunningRecordStartedEvent.ActivityIds), activityIds)
            .SetList(nameof(RunningRecordStartedEvent.TagIds), tagIds);
    }

    private static EventPayload SerializeTag(TagName name)
    {
        return new EventPayload(Policy)
            .Set(nameof(TagCreatedEvent.Name), name);
    }
}

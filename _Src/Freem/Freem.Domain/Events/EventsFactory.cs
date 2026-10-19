using Freem.Domain.Activities;
using Freem.Domain.Activities.Events;
using Freem.Domain.Activities.Events.Arhived;
using Freem.Domain.Activities.Events.Created;
using Freem.Domain.Activities.Events.Removed;
using Freem.Domain.Activities.Events.Unarchived;
using Freem.Domain.Activities.Events.Updated;
using Freem.Domain.Events.Models;
using Freem.Domain.Records;
using Freem.Domain.Records.Events;
using Freem.Domain.Records.Events.Created;
using Freem.Domain.Records.Events.Removed;
using Freem.Domain.Records.Events.Updated;
using Freem.Domain.RunningRecords;
using Freem.Domain.RunningRecords.Events;
using Freem.Domain.RunningRecords.Events.Removed;
using Freem.Domain.RunningRecords.Events.Started;
using Freem.Domain.RunningRecords.Events.Stopped;
using Freem.Domain.RunningRecords.Events.Updated;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Events;
using Freem.Domain.Tags.Events.Created;
using Freem.Domain.Tags.Events.Removed;
using Freem.Domain.Tags.Events.Updated;
using Freem.Domain.Users;
using Freem.Domain.Users.Events;
using Freem.Domain.Users.Events.Registered;
using Freem.Domain.Users.Events.SettingsChanged;

namespace Freem.Domain.Events;

public sealed class EventsFactory
{
    public static EventsFactory Instance { get; } = new();
    
    public IEntityEvent<Id, UserId> Create(
        EventId id,
        Id entityId,
        UserId userId,
        EventAction action)
    {
        return entityId switch
        {
            ActivityId activityId => CreateActivityEvent(id, activityId, userId, action),
            RecordId recordId => CreateRecordEvent(id, recordId, userId, action),
            RunningRecordId runningRecordId => CreateRunningRecordEvent(id, runningRecordId, action),
            TagId tagId => CreateTagEvent(id, tagId, userId, action),
            UserId => CreateUserEvent(id, userId, action),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<ActivityId, UserId> CreateActivityEvent(
        EventId id, ActivityId entityId, UserId userId, EventAction action)
    {
        var value = action.ToString();
        return value switch
        {
            ActivityEventActions.Created => new ActivityCreatedEvent(id, entityId, userId),
            ActivityEventActions.Updated => new ActivityUpdatedEvent(id, entityId, userId),
            ActivityEventActions.Removed => new ActivityRemovedEvent(id, entityId, userId),
            ActivityEventActions.Archived => new ActivityArchivedEvent(id, entityId, userId),
            ActivityEventActions.Unarchived => new ActivityUnarchivedEvent(id, entityId, userId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<RecordId, UserId> CreateRecordEvent(
        EventId id, RecordId entityId, UserId userId, EventAction action)
    {
        var value = action.ToString();
        return value switch
        {
            RecordEventActions.Created => new RecordCreatedEvent(id, entityId, userId),
            RecordEventActions.Updated => new RecordUpdatedEvent(id, entityId, userId),
            RecordEventActions.Removed => new RecordRemovedEvent(id, entityId, userId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<RunningRecordId, UserId> CreateRunningRecordEvent(
        EventId id, RunningRecordId entityId, EventAction action)
    {
        var value = action.ToString();
        return value switch
        {
            RunningRecordEventActions.Started => new RunningRecordStartedEvent(id, entityId),
            RunningRecordEventActions.Stopped => new RunningRecordStoppedEvent(id, entityId),
            RunningRecordEventActions.Updated => new RunningRecordUpdatedEvent(id, entityId),
            RunningRecordEventActions.Removed => new RunningRecordRemovedEvent(id, entityId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<TagId, UserId> CreateTagEvent(
        EventId id, TagId entityId, UserId userId, EventAction action)
    {
        var value = action.ToString();
        return value switch
        {
            TagEventActions.Created => new TagCreatedEvent(id, entityId, userId),
            TagEventActions.Updated => new TagUpdatedEvent(id, entityId, userId),
            TagEventActions.Removed => new TagRemovedEvent(id, entityId, userId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<UserId, UserId> CreateUserEvent(
        EventId id, UserId entityId, EventAction action)
    {
        var value = action.ToString();
        return value switch
        {
            UserEventActions.Registered => new UserRegisteredEvent(id, entityId),
            UserEventActions.SettingsChanged => new UserSettingsChanged(id, entityId),
            _ => throw new Exception()
        };
    }
}
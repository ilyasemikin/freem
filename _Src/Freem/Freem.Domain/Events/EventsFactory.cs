using Freem.Domain.Activities.Events;
using Freem.Domain.Activities.Events.Arhived;
using Freem.Domain.Activities.Events.Created;
using Freem.Domain.Activities.Events.Removed;
using Freem.Domain.Activities.Events.Unarchived;
using Freem.Domain.Activities.Events.Updated;
using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Events.Models;
using Freem.Domain.Identifiers;
using Freem.Domain.Records.Events;
using Freem.Domain.Records.Events.Created;
using Freem.Domain.Records.Events.Removed;
using Freem.Domain.Records.Events.Updated;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.RunningRecords.Events;
using Freem.Domain.RunningRecords.Events.Removed;
using Freem.Domain.RunningRecords.Events.Started;
using Freem.Domain.RunningRecords.Events.Stopped;
using Freem.Domain.RunningRecords.Events.Updated;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Tags.Events;
using Freem.Domain.Tags.Events.Created;
using Freem.Domain.Tags.Events.Removed;
using Freem.Domain.Tags.Events.Updated;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Events;
using Freem.Domain.Users.Events.Registered;
using Freem.Domain.Users.Events.SettingsChanged;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Events;

public sealed class EventsFactory
{
    public static EventsFactory Instance { get; } = new();
    
    public IEntityEvent<IEntityIdentifier, UserIdentifier> Create(
        EventIdentifier id,
        IEntityIdentifier entityId,
        UserIdentifier userId,
        EventAction action)
    {
        return entityId switch
        {
            ActivityIdentifier activityId => CreateActivityEvent(id, activityId, userId, action),
            RecordIdentifier recordId => CreateRecordEvent(id, recordId, userId, action),
            RunningRecordIdentifier runningRecordId => CreateRunningRecordEvent(id, runningRecordId, action),
            TagIdentifier tagId => CreateTagEvent(id, tagId, userId, action),
            UserIdentifier => CreateUserEvent(id, userId, action),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<ActivityIdentifier, UserIdentifier> CreateActivityEvent(
        EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId, EventAction action)
    {
        return (string)action switch
        {
            ActivityEventActions.Created => new ActivityCreatedEvent(id, entityId, userId),
            ActivityEventActions.Updated => new ActivityUpdatedEvent(id, entityId, userId),
            ActivityEventActions.Removed => new ActivityRemovedEvent(id, entityId, userId),
            ActivityEventActions.Archived => new ActivityArchivedEvent(id, entityId, userId),
            ActivityEventActions.Unarchived => new ActivityUnarchivedEvent(id, entityId, userId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<RecordIdentifier, UserIdentifier> CreateRecordEvent(
        EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId, EventAction action)
    {
        return (string)action switch
        {
            RecordEventActions.Created => new RecordCreatedEvent(id, entityId, userId),
            RecordEventActions.Updated => new RecordUpdatedEvent(id, entityId, userId),
            RecordEventActions.Removed => new RecordRemovedEvent(id, entityId, userId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<RunningRecordIdentifier, UserIdentifier> CreateRunningRecordEvent(
        EventIdentifier id, RunningRecordIdentifier entityId, EventAction action)
    {
        return (string)action switch
        {
            RunningRecordEventActions.Started => new RunningRecordStartedEvent(id, entityId),
            RunningRecordEventActions.Stopped => new RunningRecordStoppedEvent(id, entityId),
            RunningRecordEventActions.Updated => new RunningRecordUpdatedEvent(id, entityId),
            RunningRecordEventActions.Removed => new RunningRecordRemovedEvent(id, entityId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<TagIdentifier, UserIdentifier> CreateTagEvent(
        EventIdentifier id, TagIdentifier entityId, UserIdentifier userId, EventAction action)
    {
        return (string)action switch
        {
            TagEventActions.Created => new TagCreatedEvent(id, entityId, userId),
            TagEventActions.Updated => new TagUpdatedEvent(id, entityId, userId),
            TagEventActions.Removed => new TagRemovedEvent(id, entityId, userId),
            _ => throw new Exception()
        };
    }

    private static IEntityEvent<UserIdentifier, UserIdentifier> CreateUserEvent(
        EventIdentifier id, UserIdentifier entityId, EventAction action)
    {
        return (string)action switch
        {
            UserEventActions.Registered => new UserRegisteredEvent(id, entityId),
            UserEventActions.SettingsChanged => new UserSettingsChanged(id, entityId),
            _ => throw new Exception()
        };
    }
}
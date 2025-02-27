using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Events;
using Freem.Entities.Activities.Events.Arhived;
using Freem.Entities.Activities.Events.Created;
using Freem.Entities.Activities.Events.Removed;
using Freem.Entities.Activities.Events.Unarchived;
using Freem.Entities.Activities.Events.Updated;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Events;
using Freem.Entities.Records.Events.Created;
using Freem.Entities.Records.Events.Removed;
using Freem.Entities.Records.Events.Updated;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords.Events;
using Freem.Entities.RunningRecords.Events.Removed;
using Freem.Entities.RunningRecords.Events.Started;
using Freem.Entities.RunningRecords.Events.Stopped;
using Freem.Entities.RunningRecords.Events.Updated;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Tags.Events;
using Freem.Entities.Tags.Events.Created;
using Freem.Entities.Tags.Events.Removed;
using Freem.Entities.Tags.Events.Updated;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Events;
using Freem.Entities.Users.Events.PasswordCredentialsChanged;
using Freem.Entities.Users.Events.Registered;
using Freem.Entities.Users.Events.SettingsChanged;
using Freem.Entities.Users.Events.SignedIn;
using Freem.Entities.Users.Events.TelegramIntegrationChanged;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Events;

public sealed class EventsFactory
{
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
            UserEventActions.SignedIn => new UserSignedInEvent(id, entityId),
            UserEventActions.SettingsChanged => new UserSettingsChanged(id, entityId),
            UserEventActions.LoginCredentialsChanged => new UserPasswordCredentialsChangedEvent(id, entityId),
            UserEventActions.TelegramIntegrationChanged => new UserTelegramIntegrationChangedEvent(id, entityId),
            _ => throw new Exception()
        };
    }
}
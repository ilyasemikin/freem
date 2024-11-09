using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Events;
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
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Tags.Events;
using Freem.Entities.Tags.Events.Created;
using Freem.Entities.Tags.Events.Removed;
using Freem.Entities.Tags.Events.Updated;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Events;
using Freem.Entities.Users.Events.SignedIn;
using Freem.Entities.Users.Identifiers;
using Freem.Exceptions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal sealed class DatabaseEntityToEventEntityConverter 
    : IConverter<EventEntity, IEntityEvent<IEntityIdentifier, UserIdentifier>>
{
    public IEntityEvent<IEntityIdentifier, UserIdentifier> Convert(EventEntity entity)
    {
        return entity.EntityName switch
        {
            EntitiesNames.Activities.EntityName => ConvertActivity(entity),
            EntitiesNames.Records.EntityName => ConvertRecord(entity),
            EntitiesNames.RunningRecords.EntityName => ConvertRunningRecord(entity),
            EntitiesNames.Tags.EntityName => ConvertTag(entity),
            EntitiesNames.Users.EntityName => ConvertUser(entity),
            _ => throw new UnknownConstantException(entity.EntityName)
        };
    }

    private static IEntityEvent<ActivityIdentifier, UserIdentifier> ConvertActivity(EventEntity entity)
    {
        return entity.Action switch
        {
            ActivityEventActions.Created => new ActivityCreatedEvent(entity.Id, entity.EntityId, entity.UserId),
            ActivityEventActions.Updated => new ActivityUpdatedEvent(entity.Id, entity.EntityId, entity.UserId),
            ActivityEventActions.Removed => new ActivityRemovedEvent(entity.Id, entity.EntityId, entity.UserId),
            ActivityEventActions.Archived => new ActivityArchivedEvent(entity.Id, entity.EntityId, entity.UserId),
            ActivityEventActions.Unarchived => new ActivityUnarchivedEvent(entity.Id, entity.EntityId, entity.UserId),
            _ => throw new UnknownConstantException(entity.Action)
        };
    }

    private static IEntityEvent<RecordIdentifier, UserIdentifier> ConvertRecord(EventEntity entity)
    {
        return entity.Action switch
        {
            RecordEventActions.Created => new RecordCreatedEvent(entity.Id, entity.EntityId, entity.UserId),
            RecordEventActions.Updated => new RecordUpdatedEvent(entity.Id, entity.EntityId, entity.UserId),
            RecordEventActions.Removed => new RecordRemovedEvent(entity.Id, entity.EntityId, entity.UserId),
            _ => throw new UnknownConstantException(entity.Action)
        };
    }

    private static IEntityEvent<RunningRecordIdentifier, UserIdentifier> ConvertRunningRecord(EventEntity entity)
    {
        return entity.Action switch
        {
            RunningRecordEventActions.Started => new RunningRecordStartedEvent(entity.Id, entity.EntityId),
            RunningRecordEventActions.Stopped => new RunningRecordStoppedEvent(entity.Id, entity.EntityId),
            RunningRecordEventActions.Updated => new RunningRecordUpdatedEvent(entity.Id, entity.EntityId),
            RunningRecordEventActions.Removed => new RunningRecordRemovedEvent(entity.Id, entity.EntityId),
            _ => throw new UnknownConstantException(entity.Action)
        };
    }

    private static IEntityEvent<TagIdentifier, UserIdentifier> ConvertTag(EventEntity entity)
    {
        return entity.Action switch
        {
            TagEventActions.Created => new TagCreatedEvent(entity.Id, entity.EntityId, entity.UserId),
            TagEventActions.Updated => new TagUpdatedEvent(entity.Id, entity.EntityId, entity.UserId),
            TagEventActions.Removed => new TagRemovedEvent(entity.Id, entity.EntityId, entity.UserId),
            _ => throw new UnknownConstantException(entity.Action)
        };
    }

    private static IEntityEvent<UserIdentifier, UserIdentifier> ConvertUser(EventEntity entity)
    {
        return entity.Action switch
        {
            UserEventActions.SignedIn => new UserSignedInEvent(entity.Id, entity.UserId),
            _ => throw new UnknownConstantException(entity.Action)
        };
    }
}
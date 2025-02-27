using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Records.Events.Removed;

public sealed class RecordRemovedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordRemovedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Removed)
    {
    }
}
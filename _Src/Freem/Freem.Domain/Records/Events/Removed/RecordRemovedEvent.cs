using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Records.Events.Removed;

public sealed class RecordRemovedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordRemovedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Removed)
    {
    }
}
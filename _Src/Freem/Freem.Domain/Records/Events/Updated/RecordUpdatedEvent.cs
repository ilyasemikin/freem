using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Records.Events.Updated;

public sealed class RecordUpdatedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordUpdatedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Updated)
    {
    }
}
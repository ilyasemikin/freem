using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Records.Events.Updated;

public sealed class RecordUpdatedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordUpdatedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Updated)
    {
    }
}
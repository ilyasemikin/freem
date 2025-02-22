using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Records.Events.Created;

public sealed class RecordCreatedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordCreatedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Created)
    {
    }
}
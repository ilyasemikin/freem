using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Records.Events.Created;

public sealed class RecordCreatedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordCreatedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Created)
    {
    }
}
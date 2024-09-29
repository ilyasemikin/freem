using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Records.Events.Created;

public sealed class RecordCreatedEvent : EntityEvent<RecordIdentifier, UserIdentifier>
{
    public RecordCreatedEvent(EventIdentifier id, RecordIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, RecordEventActions.Created)
    {
    }
}
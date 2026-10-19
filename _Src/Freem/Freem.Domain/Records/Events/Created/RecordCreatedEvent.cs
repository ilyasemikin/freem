using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Records.Events.Created;

public sealed class RecordCreatedEvent : EntityEvent<RecordId, UserId>
{
    public RecordCreatedEvent(EventId id, RecordId entityId, UserId userId) 
        : base(id, entityId, userId, RecordEventActions.Created)
    {
    }
}
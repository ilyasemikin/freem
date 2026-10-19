using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Records.Events.Updated;

public sealed class RecordUpdatedEvent : EntityEvent<RecordId, UserId>
{
    public RecordUpdatedEvent(EventId id, RecordId entityId, UserId userId) 
        : base(id, entityId, userId, RecordEventActions.Updated)
    {
    }
}
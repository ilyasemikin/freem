using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Records.Events.Removed;

public sealed class RecordRemovedEvent : EntityEvent<RecordId, UserId>
{
    public RecordRemovedEvent(EventId id, RecordId entityId, UserId userId) 
        : base(id, entityId, userId, RecordEventActions.Removed)
    {
    }
}
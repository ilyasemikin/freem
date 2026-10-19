using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.RunningRecords.Events.Updated;

public sealed class RunningRecordUpdatedEvent : EntityEvent<RunningRecordId, UserId>
{
    public RunningRecordUpdatedEvent(EventId id, RunningRecordId entityId) 
        : base(id, entityId, entityId.ToUserId(), RunningRecordEventActions.Updated)
    {
    }
}
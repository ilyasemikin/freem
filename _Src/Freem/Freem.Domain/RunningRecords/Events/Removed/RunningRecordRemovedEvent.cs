using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.RunningRecords.Events.Removed;

public sealed class RunningRecordRemovedEvent : EntityEvent<RunningRecordId, UserId>
{
    public RunningRecordRemovedEvent(EventId id, RunningRecordId entityId) 
        : base(id, entityId, entityId.ToUserId(), RunningRecordEventActions.Removed)
    {
    }
}
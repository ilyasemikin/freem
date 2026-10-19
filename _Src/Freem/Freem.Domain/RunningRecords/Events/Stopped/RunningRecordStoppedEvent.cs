using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.RunningRecords.Events.Stopped;

public sealed class RunningRecordStoppedEvent : EntityEvent<RunningRecordId, UserId>
{
    public RunningRecordStoppedEvent(EventId id, RunningRecordId entityId) 
        : base(id, entityId, entityId.ToUserId(), RunningRecordEventActions.Stopped)
    {
    }
}
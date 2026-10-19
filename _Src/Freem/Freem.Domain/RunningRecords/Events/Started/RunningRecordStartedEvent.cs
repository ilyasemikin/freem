using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.RunningRecords.Events.Started;

public sealed class RunningRecordStartedEvent : EntityEvent<RunningRecordId, UserId>
{
    public RunningRecordStartedEvent(EventId id, RunningRecordId entityId) 
        : base(id, entityId, entityId.ToUserId(), RunningRecordEventActions.Started)
    {
    }
}
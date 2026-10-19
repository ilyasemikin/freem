using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.RunningRecords.Events.Stopped;

public sealed class RunningRecordStoppedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordStoppedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Stopped)
    {
    }
}
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.RunningRecords.Events.Stopped;

public sealed class RunningRecordStoppedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordStoppedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Stopped)
    {
    }
}
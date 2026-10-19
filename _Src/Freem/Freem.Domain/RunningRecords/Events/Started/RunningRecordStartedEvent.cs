using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.RunningRecords.Events.Started;

public sealed class RunningRecordStartedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordStartedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Started)
    {
    }
}
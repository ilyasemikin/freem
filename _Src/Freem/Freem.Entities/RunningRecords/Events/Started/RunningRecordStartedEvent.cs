using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.RunningRecords.Events.Started;

public sealed class RunningRecordStartedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordStartedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Started)
    {
    }
}
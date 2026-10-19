using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.RunningRecords.Events.Updated;

public sealed class RunningRecordUpdatedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordUpdatedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Updated)
    {
    }
}
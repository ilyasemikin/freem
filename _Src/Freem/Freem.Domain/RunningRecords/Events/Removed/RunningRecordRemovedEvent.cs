using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.RunningRecords.Events.Removed;

public sealed class RunningRecordRemovedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordRemovedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Removed)
    {
    }
}
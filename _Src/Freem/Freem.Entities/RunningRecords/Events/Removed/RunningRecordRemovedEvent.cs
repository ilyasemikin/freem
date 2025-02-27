using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.RunningRecords.Events.Removed;

public sealed class RunningRecordRemovedEvent : EntityEvent<RunningRecordIdentifier, UserIdentifier>
{
    public RunningRecordRemovedEvent(EventIdentifier id, RunningRecordIdentifier entityId) 
        : base(id, entityId, entityId, RunningRecordEventActions.Removed)
    {
    }
}
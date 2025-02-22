using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Activities.Events.Removed;

public sealed class ActivityRemovedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityRemovedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Removed)
    {
    }
}
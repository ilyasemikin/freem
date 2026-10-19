using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Activities.Events.Removed;

public sealed class ActivityRemovedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityRemovedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Removed)
    {
    }
}
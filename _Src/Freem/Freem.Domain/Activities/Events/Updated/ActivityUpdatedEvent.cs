using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Activities.Events.Updated;

public sealed class ActivityUpdatedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityUpdatedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Updated)
    {
    }
}
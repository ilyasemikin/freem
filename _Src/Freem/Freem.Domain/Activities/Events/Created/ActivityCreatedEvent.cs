using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Activities.Events.Created;

public sealed class ActivityCreatedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityCreatedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Created)
    {
    }
}
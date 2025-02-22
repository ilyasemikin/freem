using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Activities.Events.Created;

public sealed class ActivityCreatedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityCreatedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Created)
    {
    }
}
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Activities.Events.Updated;

public sealed class ActivityUpdatedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityUpdatedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Updated)
    {
    }
}
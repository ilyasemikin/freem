using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Activities.Events.Arhived;

public sealed class ActivityArchivedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityArchivedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Archived)
    {
    }
}
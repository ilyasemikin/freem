using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Activities.Events.Arhived;

public sealed class ActivityArchivedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityArchivedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Archived)
    {
    }
}
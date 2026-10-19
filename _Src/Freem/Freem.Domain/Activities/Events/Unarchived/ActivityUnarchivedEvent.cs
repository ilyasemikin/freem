using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Activities.Events.Unarchived;

public sealed class ActivityUnarchivedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityUnarchivedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Unarchived)
    {
    }
}
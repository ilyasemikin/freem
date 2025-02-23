using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Activities.Events.Unarchived;

public sealed class ActivityUnarchivedEvent : EntityEvent<ActivityIdentifier, UserIdentifier>
{
    public ActivityUnarchivedEvent(EventIdentifier id, ActivityIdentifier entityId, UserIdentifier userId) 
        : base(id, entityId, userId, ActivityEventActions.Unarchived)
    {
    }
}
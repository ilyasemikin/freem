using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Activities.Events.Removed;

public sealed class ActivityRemovedEvent : EntityEvent<ActivityId, UserId>
{
    public ActivityRemovedEvent(EventId id, ActivityId entityId, UserId userId) 
        : base(id, entityId, userId, ActivityEventActions.Removed)
    {
    }
}
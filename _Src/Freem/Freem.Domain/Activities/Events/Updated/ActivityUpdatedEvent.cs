using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Activities.Events.Updated;

public sealed class ActivityUpdatedEvent : EntityEvent<ActivityId, UserId>
{
    public ActivityUpdatedEvent(EventId id, ActivityId entityId, UserId userId) 
        : base(id, entityId, userId, ActivityEventActions.Updated)
    {
    }
}
using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Activities.Events.Created;

public sealed class ActivityCreatedEvent : EntityEvent<ActivityId, UserId>
{
    public ActivityCreatedEvent(EventId id, ActivityId entityId, UserId userId) 
        : base(id, entityId, userId, ActivityEventActions.Created)
    {
    }
}
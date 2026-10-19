using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Activities.Events.Arhived;

public sealed class ActivityArchivedEvent : EntityEvent<ActivityId, UserId>
{
    public ActivityArchivedEvent(EventId id, ActivityId entityId, UserId userId) 
        : base(id, entityId, userId, ActivityEventActions.Archived)
    {
    }
}
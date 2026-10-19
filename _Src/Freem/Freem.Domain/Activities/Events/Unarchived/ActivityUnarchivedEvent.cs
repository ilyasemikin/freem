using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Activities.Events.Unarchived;

public sealed class ActivityUnarchivedEvent : EntityEvent<ActivityId, UserId>
{
    public ActivityUnarchivedEvent(EventId id, ActivityId entityId, UserId userId) 
        : base(id, entityId, userId, ActivityEventActions.Unarchived)
    {
    }
}
using Freem.Domain.Activities;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Models.Ids;

public sealed class ActivityAndUserIds
{
    public ActivityId ActivityId { get; }
    public UserId UserId { get; }

    public ActivityAndUserIds(ActivityId activityId, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(activityId);
        ArgumentNullException.ThrowIfNull(userId);
        
        ActivityId = activityId;
        UserId = userId;
    }
}
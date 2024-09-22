using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Models.Identifiers;

public sealed class ActivityAndUserIdentifiers
{
    public ActivityIdentifier ActivityId { get; }
    public UserIdentifier UserId { get; }

    public ActivityAndUserIdentifiers(ActivityIdentifier activityId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(activityId);
        ArgumentNullException.ThrowIfNull(userId);
        
        ActivityId = activityId;
        UserId = userId;
    }
}
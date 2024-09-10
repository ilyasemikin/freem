namespace Freem.Entities.Identifiers.Multiple;

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
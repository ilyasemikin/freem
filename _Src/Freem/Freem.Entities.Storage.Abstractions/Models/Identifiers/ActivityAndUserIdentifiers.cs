using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Models.Identifiers;

public sealed class ActivityAndUserIdentifiers : IMultipleEntityIdentifier
{
    public ActivityIdentifier ActivityId { get; }
    public UserIdentifier UserId { get; }

    public IEnumerable<IEntityIdentifier> Ids
    {
        get
        {
            yield return ActivityId;
            yield return UserId;
        }
    }

    public ActivityAndUserIdentifiers(ActivityIdentifier activityId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(activityId);
        ArgumentNullException.ThrowIfNull(userId);
        
        ActivityId = activityId;
        UserId = userId;
    }
}
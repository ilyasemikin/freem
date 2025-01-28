using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.Statistics.Activities;

public sealed class ActivityStatistics
{
    public ActivityIdentifier ActivityId { get; }
    public TimeSpan Duration { get; }

    public ActivityStatistics(ActivityIdentifier activityId, TimeSpan duration)
    {
        ArgumentNullException.ThrowIfNull(activityId);
        ArgumentNullException.ThrowIfNull(duration);
        
        ActivityId = activityId;
        Duration = duration;
    }
}
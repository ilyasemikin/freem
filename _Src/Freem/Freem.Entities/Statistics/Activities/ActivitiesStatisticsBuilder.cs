using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.Statistics.Activities;

public sealed class ActivitiesStatisticsBuilder
{
    private readonly Dictionary<ActivityIdentifier, ActivityStatistics> _statistics;

    public ActivitiesStatisticsBuilder()
    {
        _statistics = new Dictionary<ActivityIdentifier, ActivityStatistics>();
    }

    public bool TryAdd(ActivityStatistics statistics)
    {
        return _statistics.TryAdd(statistics.ActivityId, statistics);
    }
    
    public ActivitiesStatistics Build()
    {
        return new ActivitiesStatistics(_statistics);
    }
}
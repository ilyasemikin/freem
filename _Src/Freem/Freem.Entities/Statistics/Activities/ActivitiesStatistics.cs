using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.Statistics.Activities;

public sealed class ActivitiesStatistics
{
    private readonly IReadOnlyDictionary<ActivityIdentifier, ActivityStatistics> _statistics;
    
    public int ActivitiesCount => _statistics.Count;
    
    public ActivitiesStatistics(IEnumerable<ActivityStatistics> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        
        var dictionary = new Dictionary<ActivityIdentifier, ActivityStatistics>();
        foreach (var statistic in statistics)
        {
            if (!dictionary.TryAdd(statistic.ActivityId, statistic))
                throw new ArgumentException($"Duplicate activity \"{statistic.ActivityId}\"");
        }
        
        _statistics = dictionary;
    }

    internal ActivitiesStatistics(IReadOnlyDictionary<ActivityIdentifier, ActivityStatistics> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        
        _statistics = statistics;
    }

    public bool TryGet(ActivityIdentifier id, [NotNullWhen(true)] out ActivityStatistics? statistics)
    {
        return _statistics.TryGetValue(id, out statistics);
    }
}
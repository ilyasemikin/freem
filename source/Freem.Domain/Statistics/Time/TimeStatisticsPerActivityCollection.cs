using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Activities;

namespace Freem.Domain.Statistics.Time;

public sealed class TimeStatisticsPerActivityCollection :
    IReadOnlyDictionary<ActivityId, TimeStatisticsPerActivity>
{
    private readonly IReadOnlyDictionary<ActivityId, TimeStatisticsPerActivity> _statistics;
    
    internal static TimeStatisticsPerActivityCollection Empty { get; } = new();
    
    public int Count => _statistics.Count;
    
    public IEnumerable<ActivityId> Keys => _statistics.Keys;
    public IEnumerable<TimeStatisticsPerActivity> Values => _statistics.Values;
    
    public TimeStatisticsPerActivity this[ActivityId key] => _statistics[key];
    
    private TimeStatisticsPerActivityCollection()
    {
        _statistics = FrozenDictionary<ActivityId, TimeStatisticsPerActivity>.Empty;
    }

    public TimeStatisticsPerActivityCollection(IEnumerable<TimeStatisticsPerActivity> statistics)
    {
        _statistics = statistics.ToDictionary(x => x.Id);
    }
    
    public bool ContainsKey(ActivityId key)
    {
        return _statistics.ContainsKey(key);
    }

    public bool TryGetValue(ActivityId key, [MaybeNullWhen(false)] out TimeStatisticsPerActivity value)
    {
        return _statistics.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<ActivityId, TimeStatisticsPerActivity>> GetEnumerator()
    {
        return _statistics.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
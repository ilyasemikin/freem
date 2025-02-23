using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Statistics.Time;

public sealed class TimeStatisticsPerActivityCollection :
    IReadOnlyDictionary<ActivityIdentifier, TimeStatisticsPerActivity>
{
    private readonly IReadOnlyDictionary<ActivityIdentifier, TimeStatisticsPerActivity> _statistics;
    
    internal static TimeStatisticsPerActivityCollection Empty { get; } = new();
    
    public int Count => _statistics.Count;
    
    public IEnumerable<ActivityIdentifier> Keys => _statistics.Keys;
    public IEnumerable<TimeStatisticsPerActivity> Values => _statistics.Values;
    
    public TimeStatisticsPerActivity this[ActivityIdentifier key] => _statistics[key];
    
    private TimeStatisticsPerActivityCollection()
    {
        _statistics = FrozenDictionary<ActivityIdentifier, TimeStatisticsPerActivity>.Empty;
    }

    public TimeStatisticsPerActivityCollection(IEnumerable<TimeStatisticsPerActivity> statistics)
    {
        _statistics = statistics.ToDictionary(x => x.Id);
    }
    
    public bool ContainsKey(ActivityIdentifier key)
    {
        return _statistics.ContainsKey(key);
    }

    public bool TryGetValue(ActivityIdentifier key, [MaybeNullWhen(false)] out TimeStatisticsPerActivity value)
    {
        return _statistics.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<ActivityIdentifier, TimeStatisticsPerActivity>> GetEnumerator()
    {
        return _statistics.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
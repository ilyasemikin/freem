using Freem.Domain.Activities;
using Freem.Time.Models;

namespace Freem.Domain.Statistics.Time;

public sealed class TimeStatisticsPerActivity : IEquatable<TimeStatisticsPerActivity>
{
    public DateTimePeriod Period { get; }
    
    public ActivityId Id { get; }
    public TimeSpan RecordedTime { get; }

    public TimeStatisticsPerActivity(DateTimePeriod period, ActivityId id, TimeSpan recordedTime)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(id);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(recordedTime, period.Duration, nameof(recordedTime));
        
        Period = period;
        
        Id = id;
        RecordedTime = recordedTime;
    }

    public bool Equals(TimeStatisticsPerActivity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return 
            Period.Equals(other.Period) && 
            Id.Equals(other.Id) && 
            RecordedTime.Equals(other.RecordedTime);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TimeStatisticsPerActivity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Period, Id, RecordedTime);
    }
}
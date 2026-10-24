using Freem.Time;

namespace Freem.Domain.Statistics.Time;

public sealed class TimeStatistics
{
    public DateTimePeriod Period { get; }

    public TimeSpan RecordedTime { get; }
    public TimeStatisticsPerActivityCollection PerActivities { get; }

    public TimeStatistics(
        DateTimePeriod period, TimeSpan recordedTime,
        TimeStatisticsPerActivityCollection? perActivities = null)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(recordedTime, period.Duration, nameof(recordedTime));

        Period = period;

        RecordedTime = recordedTime;
        PerActivities = perActivities ?? TimeStatisticsPerActivityCollection.Empty;
    }
}
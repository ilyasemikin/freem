using Freem.DateTimePeriods.Helpers;

namespace Freem.DateTimePeriods.Extensions;

public static class TimePeriodExtensions
{
    public static TimeSpan SumDuration(this IEnumerable<DateTimePeriod> periods)
    {
        var orderedPeriods = periods.OrderBy(p => p.StartAt);

        var result = TimeSpan.Zero;
        var last = DateTimeOffset.MinValue;
        foreach (var period in orderedPeriods)
        {
            var duration = period.EndAt - DateTimeOffsetComparer.Max(last, period.StartAt);

            if (duration > TimeSpan.Zero)
                result += duration;

            last = DateTimeOffsetComparer.Max(last, period.EndAt);
        }

        return result;
    }
}

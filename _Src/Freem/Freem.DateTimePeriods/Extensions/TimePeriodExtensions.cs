using Freem.DateTimePeriods.Helpers;
using System.Diagnostics.CodeAnalysis;

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

    public static bool TryUpdateStartAt(this DateTimePeriod period, DateTimeOffset startAt, [NotNullWhen(true)] out DateTimePeriod? result)
    {
        if (startAt > period.EndAt)
        {
            result = null;
            return false;
        }

        result = new DateTimePeriod(startAt, period.EndAt);
        return true;
    }

    public static bool TryUpdateEndAt(this DateTimePeriod period, DateTimeOffset endAt, [NotNullWhen(true)] out DateTimePeriod? result)
    {
        if (endAt < period.StartAt)
        {
            result = null;
            return false;
        }

        result = new DateTimePeriod(period.StartAt, endAt);
        return true;
    }
}

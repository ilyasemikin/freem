using Freem.Domain.Activities;
using Freem.Domain.Entities.Records;
using Freem.Domain.Statistics.Time;
using Freem.Time;

namespace Freem.Domain.Entities.Statistics.Time;

public static class TimeStatisticsCalculator
{
    public static TimeStatistics Calculate(DateTimePeriod period, IEnumerable<Record> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        var calculator = new Calculator(period);

        var intervals = records
            .SelectMany(record => record.Periods.Select(p => new RecordInterval(p.Period, record.Activities.Ids)))
            .OrderBy(interval => interval.Period.StartAt);

        foreach (var interval in intervals)
            calculator.Process(interval);

        return calculator.GenerateResult();
    }

    private sealed record RecordInterval(DateTimePeriod Period, IEnumerable<ActivityId> ActivityIds);

    private sealed class Calculator
    {
        private readonly DateTimePeriod _period;

        private DateTimeOffset _lastEntAt;
        private TimeSpan _result;

        private readonly Dictionary<ActivityId, DateTimeOffset> _lastEndAtByActivityIds;
        private readonly Dictionary<ActivityId, TimeSpan> _resultByActivityIds;

        public Calculator(DateTimePeriod period)
        {
            ArgumentNullException.ThrowIfNull(period);

            _period = period;

            _lastEntAt = period.StartAt;
            _result = TimeSpan.Zero;

            _lastEndAtByActivityIds = new Dictionary<ActivityId, DateTimeOffset>();
            _resultByActivityIds = new Dictionary<ActivityId, TimeSpan>();
        }

        public void Process(RecordInterval interval)
        {
            // A record is selected when any of its periods intersects the window, so other periods may lie outside it
            var clippedStartAt = DateTimeOperations.Max(_period.StartAt, interval.Period.StartAt);
            var clippedEndAt = DateTimeOperations.Min(_period.EndAt, interval.Period.EndAt);
            if (clippedEndAt <= clippedStartAt)
                return;

            var startAt = DateTimeOperations.Max(clippedStartAt, _lastEntAt);
            if (clippedEndAt > startAt)
            {
                _result += clippedEndAt - startAt;
                _lastEntAt = clippedEndAt;
            }

            foreach (var activityId in interval.ActivityIds)
            {
                if (!_lastEndAtByActivityIds.TryGetValue(activityId, out var lastEndAtByActivityId))
                    lastEndAtByActivityId = _period.StartAt;

                if (!_resultByActivityIds.ContainsKey(activityId))
                    _resultByActivityIds[activityId] = TimeSpan.Zero;

                var activityStartAt = DateTimeOperations.Max(clippedStartAt, lastEndAtByActivityId);
                if (clippedEndAt > activityStartAt)
                {
                    _resultByActivityIds[activityId] += clippedEndAt - activityStartAt;
                    _lastEndAtByActivityIds[activityId] = clippedEndAt;
                }
            }
        }

        public TimeStatistics GenerateResult()
        {
            var statisticsPerActivities =
                _resultByActivityIds.Select(p => new TimeStatisticsPerActivity(_period, p.Key, p.Value));
            var statisticsPerActivitiesCollection = new TimeStatisticsPerActivityCollection(statisticsPerActivities);
            return new TimeStatistics(_period, _result, statisticsPerActivitiesCollection);
        }
    }
}
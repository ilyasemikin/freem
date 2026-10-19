using Freem.Domain.Activities;
using Freem.Domain.Entities.Records;
using Freem.Domain.Statistics.Time;
using Freem.Time;
using Freem.Time.Models;

namespace Freem.Domain.Entities.Statistics.Time;

public static class TimeStatisticsCalculator
{
    public static TimeStatistics Calculate(DateTimePeriod period, IEnumerable<Record> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        var calculator = new Calculator(period);

        foreach (var record in records)
            calculator.Process(record);

        return calculator.GenerateResult();
    }

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

        public void Process(Record record)
        {
            if (record.Period.EndAt < _period.StartAt)
                return;

            var startAt = record.Period.StartAt;
            var endAt = DateTimeOperations.Min(_period.EndAt, record.Period.EndAt);

            if (startAt < _lastEntAt)
                startAt = _lastEntAt;

            if (endAt > _lastEntAt)
            {
                _result += endAt - startAt;
                _lastEntAt = endAt;
            }

            foreach (var activityId in record.Activities.Ids)
            {
                if (!_lastEndAtByActivityIds.TryGetValue(activityId, out var lastEndAtByActivityId))
                    lastEndAtByActivityId = _period.StartAt;

                startAt = record.Period.StartAt;
                endAt = DateTimeOperations.Min(_period.EndAt, record.Period.EndAt);

                if (startAt < lastEndAtByActivityId)
                    startAt = lastEndAtByActivityId;

                if (!_resultByActivityIds.ContainsKey(activityId))
                    _resultByActivityIds[activityId] = TimeSpan.Zero;

                if (endAt > lastEndAtByActivityId)
                {
                    _resultByActivityIds[activityId] += endAt - startAt;
                    _lastEndAtByActivityIds[activityId] = endAt;
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
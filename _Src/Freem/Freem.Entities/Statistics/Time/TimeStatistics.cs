using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Records;
using Freem.Time;
using Freem.Time.Models;

namespace Freem.Entities.Statistics.Time;

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
    
        public static TimeStatistics Calculate(DateTimePeriod period, IEnumerable<Record> records)
    {
        ArgumentNullException.ThrowIfNull(records);
        
        var calculator = new Calculator(period);

        foreach (var record in records)
            calculator.Process(record);

        return calculator.GenerateResult();
    }

    private static async Task<TimeStatistics> CalculateAsync(DateTimePeriod period, IAsyncEnumerable<Record> records)
    {
        ArgumentNullException.ThrowIfNull(records);
        
        var calculator = new Calculator(period);
        
        await foreach (var record in records)
            calculator.Process(record);
        
        return calculator.GenerateResult();
    }

    private sealed class Calculator
    {
        private readonly DateTimePeriod _period;
        
        private DateTimeOffset _lastEntAt;
        private TimeSpan _result;

        private readonly Dictionary<ActivityIdentifier, DateTimeOffset> _lastEndAtByActivityIds;
        private readonly Dictionary<ActivityIdentifier, TimeSpan> _resultByActivityIds;

        public Calculator(DateTimePeriod period)
        {
            ArgumentNullException.ThrowIfNull(period);
            
            _period = period;
            
            _lastEntAt = period.StartAt;
            _result = TimeSpan.Zero;

            _lastEndAtByActivityIds = new Dictionary<ActivityIdentifier, DateTimeOffset>();
            _resultByActivityIds = new Dictionary<ActivityIdentifier, TimeSpan>();
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

            foreach (var activityId in record.Activities.Identifiers)
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
            var statisticsPerActivities = _resultByActivityIds.Select(p => new TimeStatisticsPerActivity(_period, p.Key, p.Value));
            var statisticsPerActivitiesCollection = new TimeStatisticsPerActivityCollection(statisticsPerActivities);
            return new TimeStatistics(_period, _result, statisticsPerActivitiesCollection);
        }
    }
}
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Statistics;
using Freem.Entities.Statistics.Time;
using Freem.Entities.Users.Identifiers;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Record = Freem.Entities.Records.Record;

namespace Freem.Entities.UnitTests.Tests.Statistics;

public class TimeStatisticsTests
{
    private static readonly UserIdentifier DefaultUserId;
    private static readonly ActivityIdentifier DefaultActivityId;
    private static readonly RelatedActivitiesCollection DefaultRelatedActivities;

    static TimeStatisticsTests()
    {
        DefaultUserId = new UserIdentifier(Guid.NewGuid().ToString());
        DefaultActivityId = new ActivityIdentifier(Guid.NewGuid().ToString());
        DefaultRelatedActivities = new RelatedActivitiesCollection([DefaultActivityId]);
    }

    [Fact]
    public void Calculate_ShouldValid_WhenNoRecordsPassed()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.Zero;
        
        var records = Array.Empty<Record>();

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));

        var statistics = TimeStatistics.Calculate(period, records);

        Assert.NotNull(statistics);
        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Empty(statistics.PerActivities);
    }
    
    [Fact]
    public void Calculate_ShouldValid_WhenFewRecords()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.FromHours(4);

        var records = new[]
        {
            CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(4)),
            CreateRecord(date.ToUtcDateTime(8), date.ToUtcDateTime(10)),
        };

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));

        var statistics = TimeStatistics.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Contains(DefaultActivityId, statistics.PerActivities);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }
    
    [Fact]
    public void Calculate_ShouldValid_WhenRecordsOverlapsed()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.FromHours(6);
        
        var records = new[]
        {
            CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(4)),
            CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(6))
        };

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));
        
        var statistics = TimeStatistics.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Contains(DefaultActivityId, statistics.PerActivities);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }

    [Fact]
    public void Calculate_ShouldValid_WhenRecordsOverlapsedAndOverlappingPeriod()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.FromHours(24);

        var records = new[]
        {
            CreateRecord(date.ToUtcDateTime().AddHours(-4), date.ToUtcDateTime(4)),
            CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(6)),
            CreateRecord(date.ToUtcDateTime(4), date.AddDays(1).ToUtcDateTime(4))
        };
        
        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));
        
        var statistics = TimeStatistics.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Contains(DefaultActivityId, statistics.PerActivities);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }

    public static TheoryData<IEnumerable<Record>, TimeSpan> OneRecordIncludeOtherCases
    {
        get
        {
            var data = new TheoryData<IEnumerable<Record>, TimeSpan>();

            var date = new DateOnly(2025, 01, 01);
            
            data.Add(
                [
                    CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(2)),
                    CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(4))
                ],
                TimeSpan.FromHours(4));
            
            data.Add(
                [
                    CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(4)),
                    CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(4))
                ],
                TimeSpan.FromHours(4));
            
            data.Add(
                [
                    CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(4)),
                    CreateRecord(date.ToUtcDateTime(1), date.ToUtcDateTime(3))
                ],
                TimeSpan.FromHours(4));
            
            return data;
        }
    }
    
    [Theory]
    [MemberData(nameof(OneRecordIncludeOtherCases))]
    public void Calculate_ShouldValid_WhenOneRecordIncludeOther(IEnumerable<Record> records, TimeSpan expected)
    {
        var date = new DateOnly(2025, 01, 01);
        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));
        
        var statistics = TimeStatistics.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Contains(DefaultActivityId, statistics.PerActivities);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }

    public static TheoryData<IEnumerable<Record>, TimeSpan, IReadOnlyDictionary<ActivityIdentifier, TimeSpan>>
        RecordsHasMultipleActivitiesCases
    {
        get
        {
            var data = new TheoryData<IEnumerable<Record>, TimeSpan, IReadOnlyDictionary<ActivityIdentifier, TimeSpan>>();

            var date = new DateOnly(2025, 01, 01);
            var activityIds = Enumerable.Range(0, 2)
                .Select(_ => new ActivityIdentifier(Guid.NewGuid().ToString()))
                .ToArray();
            
            data.Add(
                [CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(2), new RelatedActivitiesCollection(activityIds))],
                TimeSpan.FromHours(2),
                new Dictionary<ActivityIdentifier, TimeSpan>
                {
                    [activityIds[0]] = TimeSpan.FromHours(2),
                    [activityIds[1]] = TimeSpan.FromHours(2)
                });
            
            data.Add(
                [
                    CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(4), new RelatedActivitiesCollection(activityIds)),
                    CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(6), new RelatedActivitiesCollection(activityIds[0])),
                    CreateRecord(date.ToUtcDateTime(10), date.ToUtcDateTime(14), new RelatedActivitiesCollection(activityIds[1])),
                ],
                TimeSpan.FromHours(10),
                new Dictionary<ActivityIdentifier, TimeSpan>
                {
                    [activityIds[0]] = TimeSpan.FromHours(6),
                    [activityIds[1]] = TimeSpan.FromHours(8)
                });
            
            return data;
        }
    }
    
    [Theory]
    [MemberData(nameof(RecordsHasMultipleActivitiesCases))]
    public void Calculate_ShouldValid_WhenRecordsHasMultipleActivities(
        IEnumerable<Record> records, 
        TimeSpan expectedTotal, IReadOnlyDictionary<ActivityIdentifier, TimeSpan> expectedByActivities)
    {
        var date = new DateOnly(2025, 01, 01);
        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));
        
        var statistics = TimeStatistics.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expectedTotal, statistics.RecordedTime);

        foreach (var (expectedActivityId, expectedActivityTime) in expectedByActivities)
        {
            Assert.Contains(expectedActivityId, statistics.PerActivities);
            Assert.Equal(expectedActivityTime, statistics.PerActivities[expectedActivityId].RecordedTime);
        }
    }

    private static Record CreateRecord(DateTimeOffset startAt, DateTimeOffset endAt, RelatedActivitiesCollection? activities = null)
    {
        activities ??= DefaultRelatedActivities;
        
        var period = new DateTimePeriod(startAt, endAt);
        var id = new RecordIdentifier(Guid.NewGuid().ToString());
        return new Record(id, DefaultUserId, activities, RelatedTagsCollection.Empty, period);
    }
}
using Freem.Domain.Activities;
using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Statistics.Time;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Time;
using Freem.Time.Extensions;
using Record = Freem.Domain.Entities.Records.Record;

namespace Freem.Domain.UnitTests.Tests.Statistics;

public class TimeStatisticsTests
{
    private static readonly UserId DefaultUserId;
    private static readonly ActivityId DefaultActivityId;
    private static readonly RelatedActivityIdsCollection DefaultRelatedActivities;

    static TimeStatisticsTests()
    {
        DefaultUserId = UserId.Generate();
        DefaultActivityId = ActivityId.Generate();
        DefaultRelatedActivities = new RelatedActivityIdsCollection([DefaultActivityId]);
    }

    [Fact]
    public void Calculate_ShouldValid_WhenNoRecordsPassed()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.Zero;
        
        var records = Array.Empty<Record>();

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));

        var statistics = TimeStatisticsCalculator.Calculate(period, records);

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

        var statistics = TimeStatisticsCalculator.Calculate(period, records);
        
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
        
        var statistics = TimeStatisticsCalculator.Calculate(period, records);
        
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
        
        var statistics = TimeStatisticsCalculator.Calculate(period, records);
        
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
        
        var statistics = TimeStatisticsCalculator.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Contains(DefaultActivityId, statistics.PerActivities);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }

    public static TheoryData<IEnumerable<Record>, TimeSpan, IReadOnlyDictionary<ActivityId, TimeSpan>>
        RecordsHasMultipleActivitiesCases
    {
        get
        {
            var data = new TheoryData<IEnumerable<Record>, TimeSpan, IReadOnlyDictionary<ActivityId, TimeSpan>>();

            var date = new DateOnly(2025, 01, 01);
            var activityIds = Enumerable.Range(0, 2)
                .Select(_ => ActivityId.Generate())
                .ToArray();
            
            data.Add(
                [CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(2), new RelatedActivityIdsCollection(activityIds))],
                TimeSpan.FromHours(2),
                new Dictionary<ActivityId, TimeSpan>
                {
                    [activityIds[0]] = TimeSpan.FromHours(2),
                    [activityIds[1]] = TimeSpan.FromHours(2)
                });
            
            data.Add(
                [
                    CreateRecord(date.ToUtcDateTime(), date.ToUtcDateTime(4), new RelatedActivityIdsCollection(activityIds)),
                    CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(6), new RelatedActivityIdsCollection(activityIds[0])),
                    CreateRecord(date.ToUtcDateTime(10), date.ToUtcDateTime(14), new RelatedActivityIdsCollection(activityIds[1])),
                ],
                TimeSpan.FromHours(10),
                new Dictionary<ActivityId, TimeSpan>
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
        TimeSpan expectedTotal, IReadOnlyDictionary<ActivityId, TimeSpan> expectedByActivities)
    {
        var date = new DateOnly(2025, 01, 01);
        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));
        
        var statistics = TimeStatisticsCalculator.Calculate(period, records);
        
        Assert.NotNull(statistics);
        Assert.Equal(expectedTotal, statistics.RecordedTime);

        foreach (var (expectedActivityId, expectedActivityTime) in expectedByActivities)
        {
            Assert.Contains(expectedActivityId, statistics.PerActivities);
            Assert.Equal(expectedActivityTime, statistics.PerActivities[expectedActivityId].RecordedTime);
        }
    }

    [Fact]
    public void Calculate_ShouldCountOnce_WhenPeriodsOfRecordsInterleave()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.FromHours(6);

        var first = CreateRecord(date.ToUtcDateTime(1), date.ToUtcDateTime(3));
        var secondPeriods = new[]
        {
            CreatePeriod(date.ToUtcDateTime(2), date.ToUtcDateTime(5)),
            CreatePeriod(date.ToUtcDateTime(8), date.ToUtcDateTime(10))
        };
        var second = new Record(
            RecordId.Generate(),
            DefaultUserId,
            DefaultRelatedActivities,
            RelatedTagIdsCollection.Empty,
            new RecordPeriodsCollection(secondPeriods));

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));

        var statistics = TimeStatisticsCalculator.Calculate(period, [second, first]);

        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }

    [Fact]
    public void Calculate_ShouldIgnorePeriodsOutsideWindow_WhenRecordHasPeriodsOnOtherDays()
    {
        var date = new DateOnly(2025, 01, 01);
        var expected = TimeSpan.FromHours(1);

        var periods = new[]
        {
            CreatePeriod(date.AddDays(-1).ToUtcDateTime(13), date.AddDays(-1).ToUtcDateTime(14)),
            CreatePeriod(date.ToUtcDateTime(13), date.ToUtcDateTime(14)),
            CreatePeriod(date.AddDays(1).ToUtcDateTime(13), date.AddDays(1).ToUtcDateTime(14))
        };
        var record = new Record(
            RecordId.Generate(),
            DefaultUserId,
            DefaultRelatedActivities,
            RelatedTagIdsCollection.Empty,
            new RecordPeriodsCollection(periods));

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));

        var statistics = TimeStatisticsCalculator.Calculate(period, [record]);

        Assert.Equal(expected, statistics.RecordedTime);
        Assert.Equal(expected, statistics.PerActivities[DefaultActivityId].RecordedTime);
    }

    [Fact]
    public void Calculate_ShouldNotAddActivity_WhenPeriodOnlyTouchesWindow()
    {
        var date = new DateOnly(2025, 01, 01);
        var otherActivityId = ActivityId.Generate();

        var records = new[]
        {
            CreateRecord(date.ToUtcDateTime(2), date.ToUtcDateTime(4)),
            CreateRecord(date.ToUtcDateTime().AddHours(-2), date.ToUtcDateTime(),
                new RelatedActivityIdsCollection([otherActivityId]))
        };

        var period = new DateTimePeriod(date.ToUtcDateTime(), TimeSpan.FromDays(1));

        var statistics = TimeStatisticsCalculator.Calculate(period, records);

        Assert.Equal(TimeSpan.FromHours(2), statistics.RecordedTime);
        Assert.DoesNotContain(otherActivityId, statistics.PerActivities);
    }

    private static RecordPeriod CreatePeriod(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        return new RecordPeriod(RecordPeriodId.Generate(), new DateTimePeriod(startAt, endAt), TimeZoneOffset.Default);
    }

    private static Record CreateRecord(DateTimeOffset startAt, DateTimeOffset endAt, RelatedActivityIdsCollection? activities = null)
    {
        activities ??= DefaultRelatedActivities;
        
        var period = new DateTimePeriod(startAt, endAt);
        var id = RecordId.Generate();
        var periodId = RecordPeriodId.Generate();
        var recordPeriod = new RecordPeriod(periodId, period, TimeZoneOffset.Default);
        var periods = new RecordPeriodsCollection(recordPeriod);
        return new Record(id, DefaultUserId, activities, RelatedTagIdsCollection.Empty, periods);
    }
}
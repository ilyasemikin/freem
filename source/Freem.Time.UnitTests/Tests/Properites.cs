using Freem.Time.UnitTests.Tests.DataFactory;

namespace Freem.Time.UnitTests.Tests;

public class Properites
{
    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public void DateTimeOffsets_ShouldHaveUtcTimeZone(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var expectedStart = (DateTimeOffset)startAt.UtcDateTime;
        var expectedEnd = (DateTimeOffset)endAt.UtcDateTime;

        var period = new DateTimePeriod(startAt, endAt);

        Assert.Equal(expectedStart, period.StartAt);
        Assert.Equal(expectedEnd, period.EndAt);
    }

    [Fact]
    public void DateTimeOffsets_ShouldKeepMilliseconds_AndEraseFiner()
    {
        var expected = new DateTimeOffset(2025, 1, 1, 0, 0, 0, 10, TimeSpan.Zero);
        var input = expected.AddTicks(9_999);

        var period = new DateTimePeriod(input, input);
        
        Assert.Equal(expected, period.StartAt);
        Assert.Equal(expected, period.EndAt);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public void Duration_ShouldBeCorrect(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var expected = endAt.UtcDateTime - startAt.UtcDateTime;

        var period = new DateTimePeriod(startAt, endAt);

        Assert.Equal(expected, period.Duration);
    }
}

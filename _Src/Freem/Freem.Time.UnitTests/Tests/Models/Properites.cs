using Freem.Time.Models;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using Xunit;

namespace Freem.Time.UnitTests.Tests.Models;

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
    public void DateTimeOffsets_ShouldEraseMilliseconds()
    {
        var input = new DateTimeOffset(2025, 1, 1, 0, 0, 0, 10, TimeSpan.Zero);
        var expected = new DateTimeOffset(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, input.Offset);

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

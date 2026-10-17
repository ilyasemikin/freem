using Freem.Time.Models;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using Xunit;

namespace Freem.Time.UnitTests.Tests.Models;

public class Methods
{
    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public static void Equals_WhenSame_ShouldReturnsTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt, endAt);

        Assert.True(first.Equals(second));
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public static void Equals_WhenSame_ShouldReturnsFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt.AddDays(1), endAt.AddDays(1));

        Assert.False(first.Equals(second));
    }
}

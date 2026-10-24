using Freem.Time.UnitTests.Tests.DataFactory;

namespace Freem.Time.UnitTests.Tests;

public class Operators
{
    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public static void EqualsOperator_WhenSame_ShouldTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt, endAt);

        Assert.True(first == second);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public static void EqualsOperator_WhenSame_ShouldFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt.AddDays(1), endAt.AddDays(1));

        Assert.False(first == second);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public static void NotEqualsOperator_WhenSame_ShouldFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt, endAt);

        Assert.False(first != second);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public static void NotEqualsOperator_WhenSame_ShouldTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt.AddDays(1), endAt.AddDays(1));

        Assert.True(first != second);
    }
}

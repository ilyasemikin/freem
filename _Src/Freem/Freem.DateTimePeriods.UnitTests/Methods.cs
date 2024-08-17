using Freem.DateTimePeriods.UnitTests.DataFactory;
using NUnit.Framework;

namespace Freem.DateTimePeriods.UnitTests;

public class Methods
{
    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public static void Equals_WhenSame_ShouldReturnsTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt, endAt);

        Assert.That(first.Equals(second), Is.True);
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public static void Equals_WhenSame_ShouldReturnsFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt.AddDays(1), endAt.AddDays(1));

        Assert.That(first.Equals(second), Is.False);
    }
}

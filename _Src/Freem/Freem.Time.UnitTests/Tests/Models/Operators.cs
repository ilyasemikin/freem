using Freem.Time.Models;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using NUnit.Framework;

namespace Freem.Time.UnitTests.Tests.Models;

public class Operators
{
    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public static void EqualsOperator_WhenSame_ShouldTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt, endAt);

        Assert.That(first == second, Is.True);
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public static void EqualsOperator_WhenSame_ShouldFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt.AddDays(1), endAt.AddDays(1));

        Assert.That(first == second, Is.False);
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public static void NotEqualsOperator_WhenSame_ShouldFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt, endAt);

        Assert.That(first != second, Is.False);
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public static void NotEqualsOperator_WhenSame_ShouldTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var first = new DateTimePeriod(startAt, endAt);
        var second = new DateTimePeriod(startAt.AddDays(1), endAt.AddDays(1));

        Assert.That(first != second, Is.True);
    }
}

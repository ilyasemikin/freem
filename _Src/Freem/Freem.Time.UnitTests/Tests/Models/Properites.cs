using Freem.Time.Models;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using NUnit.Framework;

namespace Freem.Time.UnitTests.Tests.Models;

public class Properites
{
    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public void DateTimeOffsets_ShouldHaveUtcTimeZone(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var expectedStart = (DateTimeOffset)startAt.UtcDateTime;
        var expectedEnd = (DateTimeOffset)endAt.UtcDateTime;

        var period = new DateTimePeriod(startAt, endAt);

        Assert.Multiple(() =>
        {
            Assert.That(period.StartAt, Is.EqualTo(expectedStart));
            Assert.That(period.EndAt, Is.EqualTo(expectedEnd));
        });
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public void Duration_ShouldBeCorrect(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        var expected = endAt.UtcDateTime - startAt.UtcDateTime;

        var period = new DateTimePeriod(startAt, endAt);

        Assert.That(period.Duration, Is.EqualTo(expected));
    }
}

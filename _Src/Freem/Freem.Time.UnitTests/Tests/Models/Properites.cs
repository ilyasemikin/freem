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

    [Test]
    public void DateTimeOffsets_ShouldEraseMilliseconds()
    {
        var input = new DateTimeOffset(2025, 1, 1, 0, 0, 0, 10, TimeSpan.Zero);
        var expected = new DateTimeOffset(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, input.Offset);

        var period = new DateTimePeriod(input, input);
        
        Assert.Multiple(() =>
        {
            Assert.That(period.StartAt, Is.EqualTo(expected));
            Assert.That(period.EndAt, Is.EqualTo(expected));
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

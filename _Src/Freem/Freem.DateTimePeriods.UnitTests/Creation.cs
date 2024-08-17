using Freem.DateTimePeriods.UnitTests.DataFactory;
using NUnit.Framework;

namespace Freem.DateTimePeriods.UnitTests;

public class Creation
{
    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public void Constructor_WhenParametersCorrect_ShouldNotThrow(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        void TestMethod()
        {
            new DateTimePeriod(startAt, endAt);
        }

        Assert.DoesNotThrow(TestMethod);
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateInvalidDateTimeRange))]
    public void Constructor_WhenParametersInvalid_ShouldThrow(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        void TestMethod()
        {
            new DateTimePeriod(startAt, endAt);
        }

        Assert.Throws<ArgumentException>(TestMethod);
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange))]
    public void TryCreate_WhenParametersCorrect_ShouldTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        DateTimePeriod? period = null;
        bool? result = null;

        void TestMethod()
        {
            result = DateTimePeriod.TryCreate(startAt, endAt, out period);
        }

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(TestMethod);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(period, Is.Not.Null);
        });
    }

    [TestCaseSource(typeof(DateTimeRangesFactory), nameof(DateTimeRangesFactory.CreateInvalidDateTimeRange))]
    public void TryCreate_WhenParametersInvalid_ShouldFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        DateTimePeriod? period = null;
        bool? result = null;

        void TestMethod()
        {
            result = DateTimePeriod.TryCreate(startAt, endAt, out period);
        }

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(TestMethod);
            Assert.That(result, Is.EqualTo(false));
        });
    }
}
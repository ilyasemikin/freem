using Freem.DateTimePeriods.Extensions;
using Freem.DateTimePeriods.UnitTests.DataFactory;
using NUnit.Framework;

namespace Freem.DateTimePeriods.UnitTests;

public class ExtensionsMethods
{
    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriodsWithDuration))]
    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriodsWithDuration))]
    public void SumDuration_WithTwoPeriods_ShouldCorrectSum(DateTimePeriod first, DateTimePeriod second, TimeSpan expected)
    {
        var periods = new DateTimePeriod[] { first, second };

        var duration = periods.SumDuration();

        Assert.That(duration, Is.EqualTo(expected));
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateMultiplePeriods))]
    public void SumDuration_WithMultiplePeriods_ShouldCorrectSum(IEnumerable<DateTimePeriod> periods, TimeSpan expected)
    {
        var duration = periods.SumDuration();

        Assert.That(duration, Is.EqualTo(expected));
    }
}

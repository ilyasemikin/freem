using Freem.Time.Models;
using Freem.Time.Models.Extensions;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using Xunit;

namespace Freem.Time.UnitTests.Tests.Models;

public class ExtensionsMethods
{
    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriodsWithDuration), MemberType = typeof(DateTimePeriodFactory))]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriodsWithDuration), MemberType = typeof(DateTimePeriodFactory))]
    public void SumDuration_WithTwoPeriods_ShouldCorrectSum(DateTimePeriod first, DateTimePeriod second, TimeSpan expected)
    {
        var periods = new DateTimePeriod[] { first, second };

        var duration = periods.SumDuration();

        Assert.Equal(expected, duration);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateMultiplePeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void SumDuration_WithMultiplePeriods_ShouldCorrectSum(IEnumerable<DateTimePeriod> periods, TimeSpan expected)
    {
        var duration = periods.SumDuration();

        Assert.Equal(expected, duration);
    }
}

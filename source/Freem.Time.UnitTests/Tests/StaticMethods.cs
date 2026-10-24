using Freem.Time.UnitTests.Tests.DataFactory;

namespace Freem.Time.UnitTests.Tests;

public class StaticMethods
{
    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void IsOverlapsed_WhenPeriodsOverlapsed_ShouldRetursTrue(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsOverlapsed(left, right);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void IsOverlapsed_WhenPeriodsNonOverlapsed_ShouldRetursFalse(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsOverlapsed(left, right);

        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void IsNonOverlapsed_WhenPeriodsOverlapsed_ShouldRetursFalse(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsNonOverlapsed(left, right);

        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void IsNonOverlapsed_WhenPeriodsNonOverlapsed_ShouldRetursTrue(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsNonOverlapsed(left, right);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void TryCombine_WhenPeriodsOverlapsed_ShouldReturnsTrue(DateTimePeriod first, DateTimePeriod second)
    {
        bool? result = null;
        DateTimePeriod? combined = null;

        void TestMethod()
        {
            result = DateTimePeriod.TryCombine(first, second, out combined);
        }

        TestMethod();
        Assert.True(result);
        Assert.NotNull(combined);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriods), MemberType = typeof(DateTimePeriodFactory))]
    public void TryCombine_WhenPeriodsNonOverlapsed_ShouldReturnsFalse(DateTimePeriod first, DateTimePeriod second)
    {
        bool? result = null;
        
        void TestMethod()
        {
            result = DateTimePeriod.TryCombine(first, second, out var _);
        }

        TestMethod();
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriodsWithCombined), MemberType = typeof(DateTimePeriodFactory))]
    public void TryCombine_ShouldCorrectCombined(DateTimePeriod first, DateTimePeriod second, DateTimePeriod expected)
    {
        var success = DateTimePeriod.TryCombine(first, second, out var actual);

        Assert.True(success);

        Assert.Equal(expected.StartAt, actual!.StartAt);
        Assert.Equal(expected.EndAt, actual!.EndAt);
        Assert.Equal(expected.Duration, actual!.Duration);
    }

    [Theory]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriodsWithDuration), MemberType = typeof(DateTimePeriodFactory))]
    [MemberData(nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriodsWithDuration), MemberType = typeof(DateTimePeriodFactory))]
    public void CombineDuration_ShouldCorrect(DateTimePeriod first, DateTimePeriod second, TimeSpan expected)
    {
        var duration = DateTimePeriod.CombineDuration(first, second);

        Assert.Equal(expected, duration);
    }
}

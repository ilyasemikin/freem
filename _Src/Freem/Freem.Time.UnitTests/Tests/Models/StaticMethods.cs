using Freem.Time.Models;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using NUnit.Framework;

namespace Freem.Time.UnitTests.Tests.Models;

public class StaticMethods
{
    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriods))]
    public void IsOverlapsed_WhenPeriodsOverlapsed_ShouldRetursTrue(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsOverlapsed(left, right);

        Assert.That(result, Is.True);
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriods))]
    public void IsOverlapsed_WhenPeriodsNonOverlapsed_ShouldRetursFalse(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsOverlapsed(left, right);

        Assert.That(result, Is.False);
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriods))]
    public void IsNonOverlapsed_WhenPeriodsOverlapsed_ShouldRetursFalse(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsNonOverlapsed(left, right);

        Assert.That(result, Is.False);
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriods))]
    public void IsNonOverlapsed_WhenPeriodsNonOverlapsed_ShouldRetursTrue(DateTimePeriod left, DateTimePeriod right)
    {
        var result = DateTimePeriod.IsNonOverlapsed(left, right);

        Assert.That(result, Is.True);
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriods))]
    public void TryCombine_WhenPeriodsOverlapsed_ShouldReturnsTrue(DateTimePeriod first, DateTimePeriod second)
    {
        bool? result = null;
        DateTimePeriod? combined = null;

        void TestMethod()
        {
            result = DateTimePeriod.TryCombine(first, second, out combined);
        }

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(TestMethod);
            Assert.That(result, Is.True);
            Assert.That(combined, Is.Not.Null);
        });
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriods))]
    public void TryCombine_WhenPeriodsNonOverlapsed_ShouldReturnsFalse(DateTimePeriod first, DateTimePeriod second)
    {
        bool? result = null;
        
        void TestMethod()
        {
            result = DateTimePeriod.TryCombine(first, second, out var _);
        }

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(TestMethod);
            Assert.That(result, Is.False);
        });
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriodsWithCombined))]
    public void TryCombine_ShouldCorrectCombined(DateTimePeriod first, DateTimePeriod second, DateTimePeriod expected)
    {
        var success = DateTimePeriod.TryCombine(first, second, out var actual);

        Assert.Multiple(() =>
        {
            if (!success)
                Assert.Fail();

            Assert.That(actual!.StartAt, Is.EqualTo(expected.StartAt));
            Assert.That(actual!.EndAt, Is.EqualTo(expected.EndAt));
            Assert.That(actual!.Duration, Is.EqualTo(expected.Duration));
        });
    }

    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoOverlapsedPeriodsWithDuration))]
    [TestCaseSource(typeof(DateTimePeriodFactory), nameof(DateTimePeriodFactory.CreateTwoNonOverlapsedPeriodsWithDuration))]
    public void CombineDuration_ShouldCorrect(DateTimePeriod first, DateTimePeriod second, TimeSpan expected)
    {
        var duration = DateTimePeriod.CombineDuration(first, second);

        Assert.That(duration, Is.EqualTo(expected));
    }
}

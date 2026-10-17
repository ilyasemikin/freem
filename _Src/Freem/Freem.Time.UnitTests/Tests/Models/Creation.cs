using Freem.Time.Models;
using Freem.Time.UnitTests.Tests.Models.DataFactory;
using Xunit;

namespace Freem.Time.UnitTests.Tests.Models;

public class Creation
{
    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public void Constructor_WhenParametersCorrect_ShouldNotThrow(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        void TestMethod()
        {
            new DateTimePeriod(startAt, endAt);
        }

        TestMethod();
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateInvalidDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public void Constructor_WhenParametersInvalid_ShouldThrow(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        void TestMethod()
        {
            new DateTimePeriod(startAt, endAt);
        }

        Assert.Throws<ArgumentException>(TestMethod);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateCorrectDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public void TryCreate_WhenParametersCorrect_ShouldTrue(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        DateTimePeriod? period = null;
        bool? result = null;

        void TestMethod()
        {
            result = DateTimePeriod.TryCreate(startAt, endAt, out period);
        }

        TestMethod();
        Assert.Equal(true, result);
        Assert.NotNull(period);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangesFactory.CreateInvalidDateTimeRange), MemberType = typeof(DateTimeRangesFactory))]
    public void TryCreate_WhenParametersInvalid_ShouldFalse(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        DateTimePeriod? period = null;
        bool? result = null;

        void TestMethod()
        {
            result = DateTimePeriod.TryCreate(startAt, endAt, out period);
        }

        TestMethod();
        Assert.Equal(false, result);
    }
}
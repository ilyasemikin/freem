using Freem.Domain.Users.Models;

namespace Freem.Domain.UnitTests.Tests.Users.Models;

public sealed class TimeZoneOffsetTests
{
    [Theory]
    [InlineData("00:00:00")]
    [InlineData("03:00:00")]
    [InlineData("-05:30:00")]
    [InlineData("14:00:00")]
    [InlineData("-12:00:00")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = TimeZoneOffset.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Fact]
    public void ToString_ShouldUseTimeSpanConstantFormat()
    {
        var value = TimeSpan.FromHours(1);
        var offset = new TimeZoneOffset(value);

        var actual = offset.ToString();

        Assert.Equal("01:00:00", actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("offset")]
    [InlineData("1 hour")]
    [InlineData("14:01:00")]
    [InlineData("-12:01:00")]
    [InlineData("1.00:00:00")]
    [InlineData("03:00:30")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = TimeZoneOffset.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Theory]
    [InlineData(-721)]
    [InlineData(841)]
    public void Constructor_ShouldThrow_WhenValueIsOutOfRange(int minutes)
    {
        var value = TimeSpan.FromMinutes(minutes);

        Assert.Throws<ArgumentOutOfRangeException>(() => new TimeZoneOffset(value));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenValueIsNotWholeMinutes()
    {
        var value = TimeSpan.FromSeconds(90);

        Assert.Throws<ArgumentOutOfRangeException>(() => new TimeZoneOffset(value));
    }

    [Theory]
    [InlineData(0, 2025, 3, 10, 0, 0)]
    [InlineData(180, 2025, 3, 9, 21, 0)]
    [InlineData(-300, 2025, 3, 10, 5, 0)]
    [InlineData(330, 2025, 3, 9, 18, 30)]
    [InlineData(-570, 2025, 3, 10, 9, 30)]
    [InlineData(840, 2025, 3, 9, 10, 0)]
    [InlineData(-720, 2025, 3, 10, 12, 0)]
    public void GetDayStartUtc_ShouldSubtractOffsetFromMidnight(
        int offsetMinutes, int year, int month, int day, int hour, int minute)
    {
        var value = TimeSpan.FromMinutes(offsetMinutes);
        var offset = new TimeZoneOffset(value);
        var date = new DateOnly(2025, 3, 10);

        var actual = offset.GetDayStartUtc(date);

        var expected = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Fact]
    public void Equals_ShouldCompareByValue()
    {
        var value = TimeSpan.FromHours(3);
        var first = new TimeZoneOffset(value);
        var second = new TimeZoneOffset(value);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}

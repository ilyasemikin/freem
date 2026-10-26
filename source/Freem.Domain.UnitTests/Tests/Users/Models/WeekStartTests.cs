using Freem.Domain.Users.Models;

namespace Freem.Domain.UnitTests.Tests.Users.Models;

public sealed class WeekStartTests
{
    [Theory]
    [InlineData("Monday")]
    [InlineData("Sunday")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = WeekStart.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Tuesday")]
    [InlineData("monday")]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("5")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = WeekStart.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
    {
        Assert.Throws<FormatException>(() => WeekStart.Parse("Tuesday", null));
    }

    // 2026-10-26 - понедельник, 2026-11-01 - воскресенье
    [Theory]
    [InlineData("2026-10-26", "2026-10-26")]
    [InlineData("2026-10-28", "2026-10-26")]
    [InlineData("2026-11-01", "2026-10-26")]
    [InlineData("2026-11-02", "2026-11-02")]
    [InlineData("2027-01-01", "2026-12-28")]
    public void GetWeekStart_ShouldReturnMonday_WhenWeekStartsOnMonday(string date, string expected)
    {
        var day = DateOnly.Parse(date);
        var expectedDay = DateOnly.Parse(expected);

        var actual = WeekStart.Monday.GetWeekStart(day);

        Assert.Equal(expectedDay, actual);
    }

    [Theory]
    [InlineData("2026-10-25", "2026-10-25")]
    [InlineData("2026-10-26", "2026-10-25")]
    [InlineData("2026-10-31", "2026-10-25")]
    [InlineData("2026-11-01", "2026-11-01")]
    [InlineData("2027-01-01", "2026-12-27")]
    public void GetWeekStart_ShouldReturnSunday_WhenWeekStartsOnSunday(string date, string expected)
    {
        var day = DateOnly.Parse(date);
        var expectedDay = DateOnly.Parse(expected);

        var actual = WeekStart.Sunday.GetWeekStart(day);

        Assert.Equal(expectedDay, actual);
    }

    [Fact]
    public void Default_ShouldBeMonday()
    {
        Assert.Equal(WeekStart.Monday, WeekStart.Default);
    }
}

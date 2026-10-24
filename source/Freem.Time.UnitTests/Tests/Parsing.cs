namespace Freem.Time.UnitTests.Tests;

public class Parsing
{
    [Fact]
    public void DatePeriod_Parse_ShouldRestoreFormattedValue()
    {
        var startAt = new DateOnly(2025, 01, 01);
        var endAt = new DateOnly(2025, 01, 31);
        var period = new DatePeriod(startAt, endAt);
        var value = period.ToString();

        var actual = DatePeriod.Parse(value, null);

        Assert.Equal(period, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("2025-01-01")]
    [InlineData("2025-01-01,2025-01-31,2025-02-01")]
    [InlineData("date,2025-01-31")]
    [InlineData("2025-01-31,2025-01-01")]
    public void DatePeriod_TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = DatePeriod.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void DatePeriod_Parse_ShouldThrowFormatException_WhenValueIsInvalid()
    {
        var exception = Record.Exception(() => DatePeriod.Parse("2025-01-31,2025-01-01", null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void DateTimePeriod_Parse_ShouldRestoreFormattedValue()
    {
        var startAt = new DateTimeOffset(2025, 01, 01, 10, 00, 00, TimeSpan.Zero);
        var endAt = new DateTimeOffset(2025, 01, 01, 12, 30, 00, TimeSpan.Zero);
        var period = new DateTimePeriod(startAt, endAt);
        var value = period.ToString();

        var actual = DateTimePeriod.Parse(value, null);

        Assert.Equal(period, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("2025-01-01T10:00:00Z")]
    [InlineData("time,2025-01-01T12:00:00Z")]
    [InlineData("2025-01-01T12:00:00Z,2025-01-01T10:00:00Z")]
    public void DateTimePeriod_TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = DateTimePeriod.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void DateTimePeriod_Parse_ShouldThrowFormatException_WhenValueIsInvalid()
    {
        var exception = Record.Exception(() => DateTimePeriod.Parse("2025-01-01T12:00:00Z,2025-01-01T10:00:00Z", null));

        Assert.IsType<FormatException>(exception);
    }
}

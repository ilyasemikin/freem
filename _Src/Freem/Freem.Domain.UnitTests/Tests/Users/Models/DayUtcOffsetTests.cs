using Freem.Domain.Users.Models;

namespace Freem.Domain.UnitTests.Tests.Users.Models;

public sealed class DayUtcOffsetTests
{
    [Theory]
    [InlineData("00:00:00")]
    [InlineData("03:00:00")]
    [InlineData("-05:30:00")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = DayUtcOffset.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Fact]
    public void ToString_ShouldUseTimeSpanConstantFormat()
    {
        var value = TimeSpan.FromHours(1);
        var offset = new DayUtcOffset(value);

        var actual = offset.ToString();

        Assert.Equal("01:00:00", actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("offset")]
    [InlineData("1 hour")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = DayUtcOffset.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }
}

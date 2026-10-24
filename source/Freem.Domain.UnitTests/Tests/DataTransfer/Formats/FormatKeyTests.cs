using Freem.Domain.DataTransfer.Formats;

namespace Freem.Domain.UnitTests.Tests.DataTransfer.Formats;

public sealed class FormatKeyTests
{
    [Theory]
    [InlineData("freem-json")]
    [InlineData("time-tracker-csv")]
    [InlineData("v2")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = FormatKey.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Freem-Json")]
    [InlineData("freem json")]
    [InlineData("freem_json")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => FormatKey.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void TryParse_ShouldFailure_WhenValueIsTooLong()
    {
        var value = new string('a', FormatKey.MaxLength + 1);

        var success = FormatKey.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenValuesAreSame()
    {
        var first = FormatKey.Parse("freem-json", null);
        var second = FormatKey.Parse("freem-json", null);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}

using Freem.Domain.Activities.Models;

namespace Freem.Domain.UnitTests.Tests.Activities.Models;

public sealed class ActivityColorTests
{
    [Theory]
    [InlineData("#6366f1", "#6366f1")]
    [InlineData("#6366F1", "#6366f1")]
    [InlineData("#000000", "#000000")]
    public void Parse_ShouldNormalizeToLowerCase_WhenValueIsValid(string value, string expected)
    {
        var result = ActivityColor.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("6366f1")]
    [InlineData("#6366f")]
    [InlineData("#6366f12")]
    [InlineData("#63g6f1")]
    [InlineData("red")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => ActivityColor.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenOnlyCaseDiffers()
    {
        var first = ActivityColor.Parse("#ABCDEF", null);
        var second = ActivityColor.Parse("#abcdef", null);

        Assert.Equal(first, second);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("#12345")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = ActivityColor.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }
}

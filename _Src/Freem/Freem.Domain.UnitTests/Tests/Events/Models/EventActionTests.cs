using Freem.Domain.Events.Models;

namespace Freem.Domain.UnitTests.Tests.Events.Models;

public sealed class EventActionTests
{
    [Theory]
    [InlineData("created")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = EventAction.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => EventAction.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = EventAction.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }
}

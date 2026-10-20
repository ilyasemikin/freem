using Freem.Domain.Events;

namespace Freem.Domain.UnitTests.Tests.Events;

public sealed class EventTypeTests
{
    [Fact]
    public void Parse_ShouldKeepValue_WhenValueIsValid()
    {
        var result = EventType.Parse("tags.created", null);

        Assert.Equal("tags.created", result.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => EventType.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = EventType.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenValuesAreEqual()
    {
        var first = EventType.Parse("tags.created", null);
        var second = EventType.Parse("tags.created", null);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenValuesAreDifferent()
    {
        var first = EventType.Parse("tags.created", null);
        var second = EventType.Parse("tags.updated", null);

        Assert.NotEqual(first, second);
    }
}

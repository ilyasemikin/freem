using Freem.Domain.Tags.Models;

namespace Freem.Domain.UnitTests.Tests.Tags.Models;

public sealed class TagNameTests
{
    [Theory]
    [InlineData("name")]
    [InlineData(" name ")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = TagName.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => TagName.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = TagName.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParse_ShouldSuccess_WhenLengthIsMax()
    {
        var value = new string('a', TagName.MaxLength);

        var success = TagName.TryParse(value, null, out var result);

        Assert.True(success);
        Assert.NotNull(result);
    }

    [Fact]
    public void TryParse_ShouldFailure_WhenLengthIsMoreThanMax()
    {
        var value = new string('a', TagName.MaxLength + 1);

        var success = TagName.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }
}

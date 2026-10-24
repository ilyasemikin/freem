using Freem.Domain.Records.Models;

namespace Freem.Domain.UnitTests.Tests.Records.Models;

public sealed class RecordDescriptionTests
{
    [Theory]
    [InlineData("description")]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = RecordDescription.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(null)]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => RecordDescription.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = RecordDescription.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParse_ShouldSuccess_WhenLengthIsMax()
    {
        var value = new string('a', RecordDescription.MaxLength);

        var success = RecordDescription.TryParse(value, null, out var result);

        Assert.True(success);
        Assert.NotNull(result);
    }

    [Fact]
    public void TryParse_ShouldFailure_WhenLengthIsMoreThanMax()
    {
        var value = new string('a', RecordDescription.MaxLength + 1);

        var success = RecordDescription.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }
}

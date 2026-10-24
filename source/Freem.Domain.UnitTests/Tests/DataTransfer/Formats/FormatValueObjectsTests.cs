using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;

namespace Freem.Domain.UnitTests.Tests.DataTransfer.Formats;

public sealed class FormatValueObjectsTests
{
    [Theory]
    [InlineData("json")]
    [InlineData("csv")]
    [InlineData("mp4")]
    public void FileExtension_Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = FormatFileExtension.Parse(value, null);

        Assert.Equal(value, result.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(".json")]
    [InlineData("JSON")]
    [InlineData("tar.gz")]
    public void FileExtension_Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => FormatFileExtension.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData("application/json")]
    [InlineData("text/csv")]
    public void MediaType_Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = FormatMediaType.Parse(value, null);

        Assert.Equal(value, result.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("json")]
    [InlineData("application/")]
    [InlineData("/json")]
    [InlineData("a/b/c")]
    [InlineData("application/ json")]
    public void MediaType_Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => FormatMediaType.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Name_Parse_ShouldThrowFormatException_WhenValueIsBlank(string? value)
    {
        var exception = Record.Exception(() => FormatName.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void Name_Parse_ShouldKeepValue_WhenValueIsValid()
    {
        var result = FormatName.Parse("Freem JSON", null);

        Assert.Equal("Freem JSON", result.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ImportActivityId_Parse_ShouldThrowFormatException_WhenValueIsBlank(string? value)
    {
        var exception = Record.Exception(() => ImportActivityId.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void ImportTagId_Equals_ShouldBeTrue_WhenValuesAreSame()
    {
        var first = ImportTagId.Parse("a1", null);
        var second = ImportTagId.Parse("a1", null);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void ImportIds_Equals_ShouldBeFalse_WhenTypesAreDifferent()
    {
        Id tag = ImportTagId.Parse("a1", null);
        Id activity = ImportActivityId.Parse("a1", null);

        Assert.NotEqual(tag, activity);
    }
}

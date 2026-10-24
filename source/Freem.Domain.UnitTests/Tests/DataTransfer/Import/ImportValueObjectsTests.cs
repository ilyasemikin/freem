using Freem.Domain.DataTransfer.Import;

namespace Freem.Domain.UnitTests.Tests.DataTransfer.Import;

public sealed class ImportValueObjectsTests
{
    [Fact]
    public void DocumentId_Generate_ShouldReturnDifferentValues()
    {
        var first = ImportDocumentId.Generate();
        var second = ImportDocumentId.Generate();

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void DocumentId_Parse_ShouldKeepValue()
    {
        var id = ImportDocumentId.Generate();
        var value = id.ToString();

        var result = ImportDocumentId.Parse(value, null);

        Assert.Equal(id, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void DocumentId_Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => ImportDocumentId.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void RowId_Generate_ShouldReturnDifferentValues()
    {
        var first = ImportRowId.Generate();
        var second = ImportRowId.Generate();

        Assert.NotEqual(first, second);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void RowId_Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => ImportRowId.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData("All_21 Oct Wed.csv")]
    [InlineData("export.json")]
    public void FileName_Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = ImportFileName.Parse(value, null);

        Assert.Equal(value, result.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("dir/file.csv")]
    [InlineData("dir\\file.csv")]
    public void FileName_Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => ImportFileName.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void FileName_Parse_ShouldThrowFormatException_WhenValueIsTooLong()
    {
        var value = new string('a', ImportFileName.MaxLength + 1);

        var exception = Record.Exception(() => ImportFileName.Parse(value, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData("Created")]
    [InlineData("Parsing")]
    [InlineData("Parsed")]
    [InlineData("Failed")]
    public void DocumentStatus_Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = ImportDocumentStatus.Parse(value, null);

        Assert.Equal(value, result.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Unknown")]
    [InlineData("42")]
    public void DocumentStatus_Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => ImportDocumentStatus.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }
}

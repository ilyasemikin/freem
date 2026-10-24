using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;

namespace Freem.Domain.UnitTests.Tests.Import;

public sealed class ImportDocumentTests
{
    private static ImportDocument Create(int rowsInFile, int rowsParsed)
    {
        var id = ImportDocumentId.Generate();
        var userId = UserId.Generate();
        var format = FormatKey.Parse("time-tracker", null);
        var fileName = ImportFileName.Parse("All.csv", null);
        return new ImportDocument(
            id,
            userId,
            format,
            fileName,
            TimeZoneOffset.Default,
            ImportDocumentStatus.Parsed,
            rowsInFile,
            rowsParsed);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(10, 7)]
    public void Constructor_ShouldKeepCounts_WhenCountsAreValid(int rowsInFile, int rowsParsed)
    {
        var document = Create(rowsInFile, rowsParsed);

        Assert.Equal(rowsInFile, document.RowsInFile);
        Assert.Equal(rowsParsed, document.RowsParsed);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(1, -1)]
    [InlineData(5, 6)]
    public void Constructor_ShouldThrow_WhenCountsAreInvalid(int rowsInFile, int rowsParsed)
    {
        var exception = Record.Exception(() => Create(rowsInFile, rowsParsed));

        Assert.IsAssignableFrom<ArgumentException>(exception);
    }
}

using System.Text;
using Freem.DataTransfer.ZaferErtas.TimeTracker.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.DataTransfer.ZaferErtas.TimeTracker.UnitTests;

public sealed class TimeTrackerReaderTests
{
    private const string Header =
        "ID, Time item ID, Start Date, Start Time, End Date, End Time,  Duration, Duration in milliseconds, Activity ID, Activity Name, Notes, Category ID, Category Name";

    private readonly TimeTrackerReader _reader = new();

    private static string Row(
        string id = "1",
        string start = "1 Oct 2026, 05:29",
        string end = "1 Oct 2026, 07:29",
        string activityId = "2",
        string activityName = "Work",
        string notes = " ")
    {
        return $"{id}, 10, {start}, {end},  01:00:00, 3600000, {activityId}, {activityName}, {notes}, , ";
    }

    private async Task<Result<TimeTrackerFile, Error>> ReadTextAsync(params string[] lines)
    {
        var text = string.Join('\n', lines);
        var bytes = Encoding.UTF8.GetBytes(text);
        return await ReadBytesAsync(bytes);
    }

    private async Task<Result<TimeTrackerFile, Error>> ReadBytesAsync(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return await _reader.ReadAsync(stream);
    }

    [Fact]
    public async Task ReadAsync_ShouldReadSample()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "TimeTrackerSample.csv");
        var bytes = await File.ReadAllBytesAsync(path);

        var result = await ReadBytesAsync(bytes);

        Assert.True(result.Success);
        Assert.Equal(47, result.Value.RowsInFile);
        Assert.Equal(47, result.Value.Rows.Count);
        Assert.Empty(result.Value.Issues);
    }

    [Fact]
    public async Task ReadAsync_ShouldReadRow()
    {
        var result = await ReadTextAsync(Header, Row(id: "7", activityId: "3", activityName: "Reading", notes: "Sample note"));

        Assert.True(result.Success);
        var row = Assert.Single(result.Value.Rows);
        Assert.Equal(2, row.LineNumber);
        Assert.Equal("7", row.Id);
        Assert.Equal(new DateTime(2026, 10, 1, 5, 29, 0), row.Start);
        Assert.Equal(new DateTime(2026, 10, 1, 7, 29, 0), row.End);
        Assert.Equal("3", row.ActivityId);
        Assert.Equal("Reading", row.ActivityName);
        Assert.Equal("Sample note", row.Notes);
    }

    [Fact]
    public async Task ReadAsync_ShouldKeepCommasInNotes()
    {
        var result = await ReadTextAsync(Header, Row(notes: "first, second,third"));

        Assert.True(result.Success);
        Assert.Equal("first, second,third", Assert.Single(result.Value.Rows).Notes);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnEmptyNotes_WhenNotesAreBlank()
    {
        var result = await ReadTextAsync(Header, Row(notes: " "));

        Assert.True(result.Success);
        Assert.Equal("", Assert.Single(result.Value.Rows).Notes);
    }

    [Fact]
    public async Task ReadAsync_ShouldSupportRowThroughMidnight()
    {
        var result = await ReadTextAsync(Header, Row(start: "30 Sep 2026, 23:30", end: "1 Oct 2026, 00:15"));

        Assert.True(result.Success);
        var row = Assert.Single(result.Value.Rows);
        Assert.Equal(TimeSpan.FromMinutes(45), row.End - row.Start);
    }

    [Fact]
    public async Task ReadAsync_ShouldKeepRowsWithInvalidValuesForCaller()
    {
        var result = await ReadTextAsync(
            Header,
            Row(id: "1", start: "1 Oct 2026, 08:00", end: "1 Oct 2026, 07:00"),
            Row(id: "2", activityId: " ", activityName: " "));

        Assert.True(result.Success);
        Assert.Equal(["1", "2"], result.Value.Rows.Select(row => row.Id).ToArray());
        Assert.Empty(result.Value.Issues);
    }

    [Fact]
    public async Task ReadAsync_ShouldSkipUnreadableRowsWithIssues()
    {
        var result = await ReadTextAsync(
            Header,
            Row(id: "1"),
            Row(id: "2", start: "32 Oct 2026, 05:29"),
            Row(id: " "),
            Row(id: "1"),
            "6, 10, short",
            Row(id: "8"));

        Assert.True(result.Success);
        Assert.Equal(6, result.Value.RowsInFile);
        Assert.Equal(["1", "8"], result.Value.Rows.Select(row => row.Id).ToArray());
        Assert.Equal([3, 4, 5, 6], result.Value.Issues.Select(issue => issue.LineNumber).ToArray());
    }

    [Fact]
    public async Task ReadAsync_ShouldSupportCrLfAndBom()
    {
        var text = string.Join("\r\n", Header, Row(id: "1"), Row(id: "2")) + "\r\n";
        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(text)).ToArray();

        var result = await ReadBytesAsync(bytes);

        Assert.True(result.Success);
        Assert.Equal(2, result.Value.Rows.Count);
        Assert.Equal(2, result.Value.RowsInFile);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnEmptyFile_WhenThereAreNoRows()
    {
        var result = await ReadTextAsync(Header);

        Assert.True(result.Success);
        Assert.Empty(result.Value.Rows);
        Assert.Equal(0, result.Value.RowsInFile);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnEmptyFileError_WhenStreamIsEmpty()
    {
        var result = await ReadBytesAsync([]);

        Assert.False(result.Success);
        Assert.Equal(TimeTrackerErrorCatalog.Types.EmptyFile, result.Error.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a,b,c")]
    [InlineData("ID, Time item ID, Start Date")]
    public async Task ReadAsync_ShouldReturnInvalidHeader_WhenHeaderIsInvalid(string header)
    {
        var result = await ReadTextAsync(header, Row());

        Assert.False(result.Success);
        Assert.Equal(TimeTrackerErrorCatalog.Types.InvalidHeader, result.Error.Type);
    }
}

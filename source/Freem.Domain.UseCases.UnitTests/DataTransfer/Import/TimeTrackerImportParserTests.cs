using System.Text;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.UseCases.Implementations.DataTransfer.Import;
using Freem.Domain.Users.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.UnitTests.DataTransfer.Import;

public sealed class TimeTrackerImportParserTests
{
    private const string Header =
        "ID, Time item ID, Start Date, Start Time, End Date, End Time,  Duration, Duration in milliseconds, Activity ID, Activity Name, Notes, Category ID, Category Name";

    private static readonly TimeSpan Offset = TimeSpan.FromHours(3);

    private readonly TimeTrackerImportParser _parser = new();

    private static ImportParseOptions Options => new(new TimeZoneOffset(Offset));

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

    private async Task<Result<ImportParseResult, Error>> ParseTextAsync(params string[] lines)
    {
        var text = string.Join('\n', lines);
        return await ParseBytesAsync(Encoding.UTF8.GetBytes(text));
    }

    private async Task<Result<ImportParseResult, Error>> ParseBytesAsync(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return await _parser.ParseAsync(stream, Options);
    }

    [Fact]
    public async Task ParseAsync_ShouldParseSample()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "TimeTrackerSample.csv");
        var bytes = await File.ReadAllBytesAsync(path);

        var result = await ParseBytesAsync(bytes);

        Assert.True(result.Success);
        Assert.Equal(47, result.Value.RowsInFile);
        Assert.Equal(47, result.Value.Records.Count);
        Assert.Empty(result.Value.Warnings);
        Assert.Empty(result.Value.Tags);
        var names = result.Value.Activities
            .Select(a => a.Name.ToString())
            .Order()
            .ToArray();
        Assert.Equal(["3D printing", "Administration", "Hardware", "Reading", "Work"], names);
    }

    [Fact]
    public async Task ParseAsync_ShouldMapRowToRecord()
    {
        var result = await ParseTextAsync(Header, Row(id: "7", notes: "Sample note"));

        Assert.True(result.Success);
        var record = Assert.Single(result.Value.Records);
        Assert.Equal("7", record.Id.ToString());
        Assert.Equal("Sample note", record.Name?.ToString());
        Assert.Equal("2", Assert.Single(record.ActivityIds).ToString());
        Assert.Empty(record.TagIds);
        var period = Assert.Single(record.Periods);
        var start = new DateTimeOffset(2026, 10, 1, 5, 29, 0, Offset);
        var end = new DateTimeOffset(2026, 10, 1, 7, 29, 0, Offset);
        Assert.Equal(start, period.Period.StartAt);
        Assert.Equal(end, period.Period.EndAt);
        Assert.Equal(new TimeZoneOffset(Offset), period.TimeZoneOffset);
        var activity = Assert.Single(result.Value.Activities);
        Assert.Equal("Work", activity.Name.ToString());
        Assert.Null(activity.Color);
    }

    [Fact]
    public async Task ParseAsync_ShouldKeepCommasInNotes()
    {
        var result = await ParseTextAsync(Header, Row(notes: "first, second,third"));

        Assert.True(result.Success);
        Assert.Equal("first, second,third", Assert.Single(result.Value.Records).Name?.ToString());
    }

    [Fact]
    public async Task ParseAsync_ShouldLeaveNameEmpty_WhenNotesAreBlank()
    {
        var result = await ParseTextAsync(Header, Row(notes: " "));

        Assert.True(result.Success);
        Assert.Null(Assert.Single(result.Value.Records).Name);
    }

    [Fact]
    public async Task ParseAsync_ShouldSupportRecordThroughMidnight()
    {
        var result = await ParseTextAsync(Header, Row(start: "30 Sep 2026, 23:30", end: "1 Oct 2026, 00:15"));

        Assert.True(result.Success);
        var period = Assert.Single(Assert.Single(result.Value.Records).Periods).Period;
        Assert.Equal(TimeSpan.FromMinutes(45), period.EndAt - period.StartAt);
    }

    [Fact]
    public async Task ParseAsync_ShouldKeepRow_WhenStartEqualsEnd()
    {
        var result = await ParseTextAsync(Header, Row(start: "1 Oct 2026, 05:29", end: "1 Oct 2026, 05:29"));

        Assert.True(result.Success);
        var period = Assert.Single(Assert.Single(result.Value.Records).Periods).Period;
        Assert.Equal(period.StartAt, period.EndAt);
        Assert.Empty(result.Value.Warnings);
    }

    [Fact]
    public async Task ParseAsync_ShouldTruncateLongNotesWithWarning()
    {
        var notes = new string('a', 200);

        var result = await ParseTextAsync(Header, Row(notes: notes));

        Assert.True(result.Success);
        Assert.Equal(128, Assert.Single(result.Value.Records).Name?.ToString().Length);
        var warning = Assert.Single(result.Value.Warnings);
        Assert.Equal(2, warning.LineNumber);
        Assert.Equal(1, result.Value.RowsInFile);
    }

    [Fact]
    public async Task ParseAsync_ShouldNotSplitSurrogatePair_WhenTruncatingNotes()
    {
        var notes = new string('a', 127) + "\U0001F600" + "bbb";

        var result = await ParseTextAsync(Header, Row(notes: notes));

        Assert.True(result.Success);
        var name = Assert.Single(result.Value.Records).Name?.ToString();
        Assert.Equal(new string('a', 127), name);
        Assert.Single(result.Value.Warnings);
    }

    [Fact]
    public async Task ParseAsync_ShouldSkipInvalidRowsWithWarnings()
    {
        var result = await ParseTextAsync(
            Header,
            Row(id: "1"),
            Row(id: "2", start: "32 Oct 2026, 05:29"),
            Row(id: "3", start: "1 Oct 2026, 08:00", end: "1 Oct 2026, 07:00"),
            Row(id: "4", activityId: " "),
            Row(id: "5", activityName: " "),
            Row(id: "1"),
            "6, 10, short",
            Row(id: "8"));

        Assert.True(result.Success);
        Assert.Equal(8, result.Value.RowsInFile);
        Assert.Equal(["1", "8"], result.Value.Records.Select(r => r.Id.ToString()).ToArray());
        Assert.Equal([3, 4, 5, 6, 7, 8], result.Value.Warnings.Select(w => w.LineNumber).ToArray());
    }

    [Fact]
    public async Task ParseAsync_ShouldKeepFirstActivityName_WhenIdRepeatsWithAnotherName()
    {
        var result = await ParseTextAsync(
            Header,
            Row(id: "1", activityName: "Work"),
            Row(id: "2", activityName: "Job"));

        Assert.True(result.Success);
        Assert.Equal("Work", Assert.Single(result.Value.Activities).Name.ToString());
        Assert.Equal(2, result.Value.Records.Count);
        Assert.Equal(3, Assert.Single(result.Value.Warnings).LineNumber);
    }

    [Fact]
    public async Task ParseAsync_ShouldSupportCrLfAndBom()
    {
        var text = string.Join("\r\n", Header, Row(id: "1"), Row(id: "2")) + "\r\n";
        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(text)).ToArray();

        var result = await ParseBytesAsync(bytes);

        Assert.True(result.Success);
        Assert.Equal(2, result.Value.Records.Count);
        Assert.Equal(2, result.Value.RowsInFile);
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnEmptyResult_WhenThereAreNoRows()
    {
        var result = await ParseTextAsync(Header);

        Assert.True(result.Success);
        Assert.Empty(result.Value.Records);
        Assert.Equal(0, result.Value.RowsInFile);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a,b,c")]
    [InlineData("ID, Time item ID, Start Date")]
    public async Task ParseAsync_ShouldReturnInvalidRequest_WhenHeaderIsInvalid(string header)
    {
        var result = await ParseTextAsync(header, Row());

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.InvalidRequest, result.Error.Type);
    }
}

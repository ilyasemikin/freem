using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Import;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Domain.UseCases.UnitTests.Import;

public sealed class ImportDocumentFactoryTests
{
    private static ImportRecord CreateRecord(string id)
    {
        var recordId = ImportRecordId.Parse(id, null);
        ImportActivityId[] activityIds = [ImportActivityId.Parse("a1", null)];
        ImportTagId[] tagIds = [];
        var start = new DateTimeOffset(2026, 10, 1, 8, 0, 0, TimeSpan.Zero);
        var period = new DateTimePeriod(start, start.AddMinutes(10));
        ImportRecordPeriod[] periods = [new(period, TimeZoneOffset.Default)];
        return new ImportRecord(recordId, null, null, activityIds, tagIds, periods);
    }

    private static ImportParseResult CreateResult(int rowsInFile)
    {
        var tagName = TagName.Parse("Work", null);
        ImportTag[] tags = [new(ImportTagId.Parse("t1", null), tagName)];
        var activityName = ActivityName.Parse("Coding", null);
        IReadOnlyList<ImportTagId> tagIds = [ImportTagId.Parse("t1", null)];
        ImportActivity[] activities = [new(ImportActivityId.Parse("a1", null), activityName, null, ActivityStatus.Active, tagIds)];
        ImportRecord[] records = [CreateRecord("r1"), CreateRecord("r2")];
        return new ImportParseResult(tags, activities, records, rowsInFile, []);
    }

    [Fact]
    public void CreateDocument_ShouldCreateParsedDocumentWithActivitiesTagsAndCounts()
    {
        var result = CreateResult(rowsInFile: 5);
        var userId = UserId.Generate();
        var format = FormatKey.Parse("time-tracker", null);
        var fileName = ImportFileName.Parse("All.csv", null);
        var offset = new TimeZoneOffset(TimeSpan.FromHours(3));

        var document = ImportDocumentFactory.CreateDocument(result, userId, format, fileName, offset);

        Assert.Equal(ImportDocumentStatus.Parsed, document.Status);
        Assert.Equal(userId, document.UserId);
        Assert.Equal(format, document.Format);
        Assert.Equal(fileName, document.FileName);
        Assert.Equal(offset, document.TimeZoneOffset);
        Assert.Equal(5, document.RowsInFile);
        Assert.Equal(2, document.RowsParsed);

        var tags = ImportDocumentFactory.CreateTags(result);
        var activities = ImportDocumentFactory.CreateActivities(result);
        var tag = Assert.Single(tags);
        Assert.Null(tag.MappedTagId);
        var activity = Assert.Single(activities);
        Assert.Null(activity.MappedActivityId);
    }

    [Fact]
    public void CreateRows_ShouldCreateRowPerRecordInOrder()
    {
        var result = CreateResult(rowsInFile: 2);
        var documentId = ImportDocumentId.Generate();

        var rows = ImportDocumentFactory.CreateRows(documentId, result);

        Assert.Equal(["r1", "r2"], rows.Select(r => r.Record.Id.ToString()).ToArray());
        Assert.All(rows, r => Assert.Equal(documentId, r.DocumentId));
        Assert.Equal(2, rows.Select(r => r.Id).Distinct().Count());
    }

    [Fact]
    public void ParseResult_ShouldThrow_WhenRowsInFileIsLessThanRecords()
    {
        var exception = Record.Exception(() => CreateResult(rowsInFile: 1));

        Assert.IsAssignableFrom<ArgumentException>(exception);
    }
}

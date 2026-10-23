using Freem.Domain.Entities.Import;
using Freem.Domain.Import;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Domain.UnitTests.Tests.Import;

public sealed class ImportRowTests
{
    private static ImportRecord CreateRecord(params ImportRecordPeriod[] periods)
    {
        var id = ImportRecordId.Parse("1", null);
        ImportActivityId[] activityIds = [];
        ImportTagId[] tagIds = [];
        return new ImportRecord(id, null, null, activityIds, tagIds, periods);
    }

    [Fact]
    public void Constructor_ShouldSuccess_WhenRecordHasPeriod()
    {
        var now = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var period = new DateTimePeriod(now, now.AddMinutes(1));
        var recordPeriod = new ImportRecordPeriod(period, TimeZoneOffset.Default);
        var record = CreateRecord(recordPeriod);
        var id = ImportRowId.Generate();
        var documentId = ImportDocumentId.Generate();

        var row = new ImportRow(id, documentId, record);

        Assert.Equal(id, row.Id);
        Assert.Equal(documentId, row.DocumentId);
        Assert.Same(record, row.Record);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenRecordHasNoPeriods()
    {
        var record = CreateRecord();
        var id = ImportRowId.Generate();
        var documentId = ImportDocumentId.Generate();

        var exception = Record.Exception(() => new ImportRow(id, documentId, record));

        Assert.IsType<ArgumentException>(exception);
    }
}

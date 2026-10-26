using System.Text;
using Freem.Domain.Activities;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.Events;
using Freem.Domain.Records.Sources;
using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.List;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.Contracts.DataTransfer.Export;
using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Apply;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Get;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListActivities;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListRows;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListTags;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.UpdateActivityMapping;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.UpdateTagMapping;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Upload;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Records.List;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ImportApplyUseCasesTests : UseCaseTestBase
{
    private static readonly FormatKey TimeTrackerKey = FormatKey.Parse("time-tracker", null);
    private static readonly FormatKey NativeJsonKey = FormatKey.Parse("freem-json", null);
    private static readonly ImportFileName FileName = ImportFileName.Parse("import.dat", null);

    private HandlerContext _context = null!;

    public ImportApplyUseCasesTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        _context = await CreateContextAsync();
    }

    private async Task<HandlerContext> CreateContextAsync()
    {
        using var filler = Context.CreateExecutor();
        var userId = await filler.Users.RegisterAsync();
        return new HandlerContext(userId);
    }

    private async Task<ImportDocument> UploadAsync(HandlerContext context, FormatKey format, Stream content)
    {
        var request = new UploadImportRequest(format, FileName, content);
        var response = await Context.ExecuteAsync<UploadImportRequest, Result<UploadImportResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value.Document;
    }

    private async Task<ImportDocument> UploadSampleAsync(HandlerContext context)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "TimeTrackerSample.csv");
        await using var content = File.OpenRead(path);
        return await UploadAsync(context, TimeTrackerKey, content);
    }

    private async Task<ImportDocument> UploadTextAsync(HandlerContext context, string text)
    {
        await using var content = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return await UploadAsync(context, TimeTrackerKey, content);
    }

    private async Task<ImportDocument> GetAsync(HandlerContext context, ImportDocumentId id)
    {
        var request = new GetImportRequest(id);
        var response = await Context.ExecuteAsync<GetImportRequest, Result<GetImportResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value.Document;
    }

    private async Task<IReadOnlyList<ImportDocumentActivity>> ListActivitiesAsync(HandlerContext context, ImportDocumentId id)
    {
        var request = new ListImportActivityRequest(id, Limit.MaxValue, Offset.DefaultValue);
        var response = await Context.ExecuteAsync<ListImportActivityRequest, Result<ListImportActivityResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value.Activities;
    }

    private async Task<IReadOnlyList<ImportDocumentTag>> ListTagsAsync(HandlerContext context, ImportDocumentId id)
    {
        var request = new ListImportTagRequest(id, Limit.MaxValue, Offset.DefaultValue);
        var response = await Context.ExecuteAsync<ListImportTagRequest, Result<ListImportTagResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value.Tags;
    }

    private async Task<IReadOnlyList<ImportRow>> ListRowsAsync(HandlerContext context, ImportDocumentId id)
    {
        var request = new ListImportRowRequest(id, Limit.MaxValue, Offset.DefaultValue);
        var response = await Context.ExecuteAsync<ListImportRowRequest, Result<ListImportRowResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value.Rows;
    }

    private async Task<Result<Error>> UpdateActivityMappingAsync(
        HandlerContext context, ImportDocumentId id, ImportActivityId importActivityId, ActivityId? activityId)
    {
        var request = new UpdateImportActivityMappingRequest(id, importActivityId, activityId);
        return await Context.ExecuteAsync<UpdateImportActivityMappingRequest, Result<Error>>(context, request);
    }

    private async Task<Result<Error>> UpdateTagMappingAsync(
        HandlerContext context, ImportDocumentId id, ImportTagId importTagId, TagId? tagId)
    {
        var request = new UpdateImportTagMappingRequest(id, importTagId, tagId);
        return await Context.ExecuteAsync<UpdateImportTagMappingRequest, Result<Error>>(context, request);
    }

    /// <summary>
    /// Maps every import activity of the document to a new activity of the user created for the test.
    /// </summary>
    private async Task<IReadOnlyList<ActivityId>> UpdateAllActivityMappingsAsync(HandlerContext context, ImportDocument document)
    {
        using var filler = Context.CreateExecutor();
        var activityIds = new List<ActivityId>();
        var importActivities = await ListActivitiesAsync(context, document.Id);
        foreach (var importActivity in importActivities)
        {
            var activity = await filler.Activities.CreateAsync(context);
            var result = await UpdateActivityMappingAsync(context, document.Id, importActivity.Activity.Id, activity.Id);
            Assert.True(result.Success);
            activityIds.Add(activity.Id);
        }

        return activityIds;
    }

    private async Task<Result<ApplyImportResponse, Error>> ApplyAsync(HandlerContext context, ApplyImportRequest request)
    {
        return await Context.ExecuteAsync<ApplyImportRequest, Result<ApplyImportResponse, Error>>(context, request);
    }

    private async Task<ListRecordResponse> ListRecordsAsync(HandlerContext context)
    {
        var request = new ListRecordRequest(Limit.MaxValue, Offset.DefaultValue);
        var response = await Context.ExecuteAsync<ListRecordRequest, Result<ListRecordResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value;
    }

    private async Task<ListActivityResponse> ListActivitiesAsync(HandlerContext context)
    {
        var request = new ListActivityRequest(Limit.MaxValue, Offset.DefaultValue);
        var response = await Context.ExecuteAsync<ListActivityRequest, Result<ListActivityResponse, Error>>(context, request);
        Assert.True(response.Success);
        return response.Value;
    }

    [Fact]
    public async Task Apply_ShouldCreateRecordsWithMappedActivities_ForAllRowsOfSample()
    {
        var document = await UploadSampleAsync(_context);
        var activityIds = await UpdateAllActivityMappingsAsync(_context, document);

        var response = await ApplyAsync(_context, ApplyImportRequest.ForAllRows(document.Id));

        Assert.True(response.Success);
        Assert.Equal(47, response.Value.RecordsCreated);
        Assert.Empty(response.Value.SkippedRowIds);

        var records = await ListRecordsAsync(_context);
        Assert.Equal(47, (int)records.TotalCount);
        Assert.All(records.Records, r => Assert.All(r.Activities.Ids, id => Assert.Contains(id, activityIds)));
        var activities = await ListActivitiesAsync(_context);
        Assert.Equal(5, (int)activities.TotalCount);

        var stored = await GetAsync(_context, document.Id);
        Assert.Equal(ImportDocumentStatus.Parsed, stored.Status);

        var eventsRequest = new ListEventRequest(Limit.MaxValue, Offset.DefaultValue) { EntityType = EntityType.Record };
        var events = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, eventsRequest);
        Assert.True(events.Success);
        Assert.Equal(47, (int)events.Value.TotalCount);
    }

    [Fact]
    public async Task Apply_ShouldCreateRecordsAgain_WhenAppliedTwice()
    {
        var document = await UploadSampleAsync(_context);
        await UpdateAllActivityMappingsAsync(_context, document);
        var first = await ApplyAsync(_context, ApplyImportRequest.ForAllRows(document.Id));

        var second = await ApplyAsync(_context, ApplyImportRequest.ForAllRows(document.Id));

        Assert.True(first.Success);
        Assert.True(second.Success);
        Assert.Equal(47, second.Value.RecordsCreated);
        var records = await ListRecordsAsync(_context);
        Assert.Equal(94, (int)records.TotalCount);
    }

    [Fact]
    public async Task Apply_ShouldCreateRecordsOnlyForSelectedRows_AndRequireOnlyTheirMappings()
    {
        using var filler = Context.CreateExecutor();
        var activity = await filler.Activities.CreateAsync(_context);
        var document = await UploadSampleAsync(_context);
        var rows = await ListRowsAsync(_context, document.Id);
        var activityKey = rows[0].Record.ActivityIds[0];
        var selected = rows
            .Where(r => r.Record.ActivityIds.Contains(activityKey))
            .Take(2)
            .ToArray();
        var mapResult = await UpdateActivityMappingAsync(_context, document.Id, activityKey, activity.Id);
        Assert.True(mapResult.Success);
        var selectedIds = selected.Select(r => r.Id).ToArray();

        var response = await ApplyAsync(_context, ApplyImportRequest.ForRows(document.Id, selectedIds));

        Assert.True(response.Success);
        Assert.Equal(2, response.Value.RecordsCreated);
        var records = await ListRecordsAsync(_context);
        Assert.Equal(2, (int)records.TotalCount);
        Assert.All(records.Records, r => Assert.Equal([activity.Id], r.Activities.Ids));
        var expectedPeriods = selected.Select(r => r.Record.Periods[0].Period).OrderBy(p => p.StartAt);
        var actualPeriods = records.Records.Select(r => r.Periods[0].Period).OrderBy(p => p.StartAt);
        Assert.Equal(expectedPeriods, actualPeriods);
        var expectedSources = selected
            .Select(r => new ImportRecordSource(document.Id, r.Id, document.Format, r.Record.Id))
            .OrderBy(s => s.RowId.ToString());
        var actualSources = records.Records
            .Select(r => Assert.IsType<ImportRecordSource>(r.Source))
            .OrderBy(s => s.RowId.ToString());
        Assert.Equal(expectedSources, actualSources);
    }

    [Fact]
    public async Task Apply_ShouldReturnUnmappedEntities_AndWriteNothing_WhenActivityIsNotMapped()
    {
        using var filler = Context.CreateExecutor();
        var activity = await filler.Activities.CreateAsync(_context);
        var document = await UploadSampleAsync(_context);
        var importActivities = await ListActivitiesAsync(_context, document.Id);
        var mapResult = await UpdateActivityMappingAsync(_context, document.Id, importActivities[0].Activity.Id, activity.Id);
        Assert.True(mapResult.Success);

        var response = await ApplyAsync(_context, ApplyImportRequest.ForAllRows(document.Id));

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.ImportUnmappedEntities, response.Error.Type);
        Assert.NotNull(response.Error.Details);
        Assert.Equal(4, response.Error.Details.Count());
        var records = await ListRecordsAsync(_context);
        Assert.Equal(0, (int)records.TotalCount);
    }

    [Fact]
    public async Task Apply_ShouldSkipRowsWithZeroDuration()
    {
        const string text =
            "ID, Time item ID, Start Date, Start Time, End Date, End Time,  Duration, Duration in milliseconds, Activity ID, Activity Name, Notes, Category ID, Category Name\n" +
            "1, 1, 1 Oct 2026, 05:00, 1 Oct 2026, 06:00,  01:00:00, 1, 2, Work,  , , \n" +
            "2, 2, 1 Oct 2026, 07:00, 1 Oct 2026, 07:00,  00:00:20, 1, 2, Work,  , , \n";
        var document = await UploadTextAsync(_context, text);
        await UpdateAllActivityMappingsAsync(_context, document);
        var rows = await ListRowsAsync(_context, document.Id);
        var zeroRow = rows.Single(r => r.Record.Id.ToString() == "2");

        var response = await ApplyAsync(_context, ApplyImportRequest.ForAllRows(document.Id));

        Assert.True(response.Success);
        Assert.Equal(1, response.Value.RecordsCreated);
        Assert.Equal([zeroRow.Id], response.Value.SkippedRowIds);
    }

    [Fact]
    public async Task Apply_ShouldReturnRelatedEntitiesNotFound_WhenRowIsNotInImport()
    {
        var document = await UploadSampleAsync(_context);
        await UpdateAllActivityMappingsAsync(_context, document);
        ImportRowId[] rowIds = [ImportRowId.Generate()];

        var response = await ApplyAsync(_context, ApplyImportRequest.ForRows(document.Id, rowIds));

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ApplyAndUpdateMapping_ShouldNotAllowAccessToAnotherUsersImport()
    {
        var document = await UploadSampleAsync(_context);
        var other = await CreateContextAsync();
        using var filler = Context.CreateExecutor();
        var otherActivity = await filler.Activities.CreateAsync(other);
        var importActivities = await ListActivitiesAsync(_context, document.Id);
        var importActivityId = importActivities[0].Activity.Id;

        var apply = await ApplyAsync(other, ApplyImportRequest.ForAllRows(document.Id));
        var mappingResult = await UpdateActivityMappingAsync(other, document.Id, importActivityId, otherActivity.Id);

        Assert.False(apply.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, apply.Error.Type);
        Assert.False(mappingResult.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, mappingResult.Error.Type);
    }

    [Fact]
    public async Task UpdateActivityMapping_ShouldReturnNotFound_WhenActivityBelongsToAnotherUser()
    {
        var document = await UploadSampleAsync(_context);
        var other = await CreateContextAsync();
        using var filler = Context.CreateExecutor();
        var foreign = await filler.Activities.CreateAsync(other);
        var importActivities = await ListActivitiesAsync(_context, document.Id);
        var importActivityId = importActivities[0].Activity.Id;

        var result = await UpdateActivityMappingAsync(_context, document.Id, importActivityId, foreign.Id);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
        var stored = await ListActivitiesAsync(_context, document.Id);
        Assert.Null(stored[0].MappedActivityId);
    }

    [Fact]
    public async Task UpdateActivityMapping_ShouldReturnNotFound_WhenImportActivityIsNotInImport()
    {
        using var filler = Context.CreateExecutor();
        var activity = await filler.Activities.CreateAsync(_context);
        var document = await UploadSampleAsync(_context);
        var importActivityId = ImportActivityId.Parse("missing", null);

        var result = await UpdateActivityMappingAsync(_context, document.Id, importActivityId, activity.Id);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task UpdateActivityMapping_ShouldRemoveMapping_WhenActivityIsNull()
    {
        using var filler = Context.CreateExecutor();
        var activity = await filler.Activities.CreateAsync(_context);
        var document = await UploadSampleAsync(_context);
        var importActivities = await ListActivitiesAsync(_context, document.Id);
        var importActivityId = importActivities[0].Activity.Id;
        await UpdateActivityMappingAsync(_context, document.Id, importActivityId, activity.Id);

        var result = await UpdateActivityMappingAsync(_context, document.Id, importActivityId, null);

        Assert.True(result.Success);
        var stored = await ListActivitiesAsync(_context, document.Id);
        Assert.Null(stored[0].MappedActivityId);
    }

    [Fact]
    public async Task UpdateTagMapping_ShouldReturnNotFound_WhenTagBelongsToAnotherUser()
    {
        var document = await ExportAndUploadAsync(_context, _context);
        var other = await CreateContextAsync();
        using var filler = Context.CreateExecutor();
        var foreign = await filler.Tags.CreateAsync(other);
        var importTags = await ListTagsAsync(_context, document.Id);
        var importTagId = importTags[0].Tag.Id;

        var result = await UpdateTagMappingAsync(_context, document.Id, importTagId, foreign.Id);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task Apply_ShouldReproduceExportedRecordInAccountWithMappedActivitiesAndTags()
    {
        var target = await CreateContextAsync();
        var document = await ExportAndUploadAsync(_context, target);
        using var filler = Context.CreateExecutor();
        var importTags = await ListTagsAsync(target, document.Id);
        var targetTags = await filler.Tags.CreateManyAsync(target, importTags.Count);
        for (var i = 0; i < importTags.Count; i++)
        {
            var mapResult = await UpdateTagMappingAsync(target, document.Id, importTags[i].Tag.Id, targetTags[i].Id);
            Assert.True(mapResult.Success);
        }
        var activityIds = await UpdateAllActivityMappingsAsync(target, document);

        var response = await ApplyAsync(target, ApplyImportRequest.ForAllRows(document.Id));

        Assert.True(response.Success);
        Assert.Equal(1, response.Value.RecordsCreated);
        var exportedRecords = await ListRecordsAsync(_context);
        var targetRecords = await ListRecordsAsync(target);
        var exportedRecord = Assert.Single(exportedRecords.Records);
        var targetRecord = Assert.Single(targetRecords.Records);
        Assert.Equal(exportedRecord.Name, targetRecord.Name);
        Assert.Equal(exportedRecord.Periods[0].Period, targetRecord.Periods[0].Period);
        Assert.Equal(exportedRecord.Periods[0].TimeZoneOffset, targetRecord.Periods[0].TimeZoneOffset);
        Assert.Equal(activityIds.Order(IdComparer).ToArray(), targetRecord.Activities.Ids.Order(IdComparer).ToArray());
        var exportedTagKey = exportedRecord.Tags.Ids.Single().ToString();
        var exportedTagIndex = importTags
            .Select(t => t.Tag.Id.ToString())
            .ToList()
            .IndexOf(exportedTagKey);
        Assert.Equal([targetTags[exportedTagIndex].Id], targetRecord.Tags.Ids);
    }

    private static readonly IComparer<Id> IdComparer = Comparer<Id>.Create(
        (left, right) => StringComparer.Ordinal.Compare(left.ToString(), right.ToString()));

    private async Task<ImportDocument> ExportAndUploadAsync(HandlerContext exporter, HandlerContext target)
    {
        using var filler = Context.CreateExecutor();
        var tags = await filler.Tags.CreateManyAsync(exporter, 2);
        var activities = await filler.Activities.CreateManyAsync(exporter, 2);
        var now = DateTimeOffset.UtcNow;
        var period = new DateTimePeriod(now.AddHours(-3), now.AddHours(-2));
        var newPeriod = new NewRecordPeriod(period);
        var activityIds = new RelatedActivityIdsCollection(activities.Select(a => a.Id));
        var recordTags = new RelatedTagIdsCollection([tags[0].Id]);
        var createRequest = new CreateRecordRequest([newPeriod], activityIds)
        {
            Tags = recordTags
        };
        await filler.Records.CreateAsync(exporter, createRequest);

        using var exported = new MemoryStream();
        var exportRequest = new ExportDataRequest(NativeJsonKey, _ => exported);
        var exportResult = await Context.ExecuteAsync<ExportDataRequest, Result<ExportDataResponse, Error>>(exporter, exportRequest);
        Assert.True(exportResult.Success);
        exported.Position = 0;

        return await UploadAsync(target, NativeJsonKey, exported);
    }
}

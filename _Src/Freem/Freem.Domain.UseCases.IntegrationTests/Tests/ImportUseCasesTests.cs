using System.Text;
using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Export;
using Freem.Domain.UseCases.Contracts.Imports.Get;
using Freem.Domain.UseCases.Contracts.Imports.List;
using Freem.Domain.UseCases.Contracts.Imports.ListRows;
using Freem.Domain.UseCases.Contracts.Imports.Remove;
using Freem.Domain.UseCases.Contracts.Imports.Upload;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Domain.Users.Models;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ImportUseCasesTests : UseCaseTestBase
{
    private static readonly FormatKey TimeTrackerKey = FormatKey.Parse("time-tracker", null);
    private static readonly FormatKey NativeJsonKey = FormatKey.Parse("freem-json", null);
    private static readonly ImportFileName FileName = ImportFileName.Parse("import.dat", null);

    private HandlerContext _context = null!;

    public ImportUseCasesTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
    }

    private static Stream OpenSample()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "TimeTrackerSample.csv");
        return File.OpenRead(path);
    }

    private async Task<Result<UploadImportResponse, Error>> UploadSampleAsync(
        HandlerContext context,
        TimeZoneOffset? offset = null)
    {
        await using var content = OpenSample();
        var request = new UploadImportRequest(TimeTrackerKey, FileName, content, offset);
        return await Context.ExecuteAsync<UploadImportRequest, Result<UploadImportResponse, Error>>(context, request);
    }

    private async Task<Result<ListImportResponse, Error>> ListAsync(HandlerContext context)
    {
        return await Context.ExecuteAsync<ListImportRequest, Result<ListImportResponse, Error>>(
            context,
            new ListImportRequest());
    }

    private async Task<HandlerContext> CreateOtherContextAsync()
    {
        using var filler = Context.CreateExecutor();
        var userId = await filler.Users.RegisterAsync();
        return new HandlerContext(userId);
    }

    [Fact]
    public async Task Upload_ShouldSaveSampleAsParsedDocumentWithRows()
    {
        var offset = new TimeZoneOffset(TimeSpan.FromHours(3));

        var response = await UploadSampleAsync(_context, offset);

        Assert.True(response.Success);
        var document = response.Value.Document;
        Assert.Equal(ImportDocumentStatus.Parsed, document.Status);
        Assert.Equal(47, document.RowsInFile);
        Assert.Equal(47, document.RowsParsed);
        Assert.Equal(offset, document.TimeZoneOffset);
        Assert.Equal(5, document.Activities.Count);
        Assert.All(document.Activities, a => Assert.Null(a.Resolution));
        Assert.Empty(response.Value.Warnings);

        var getRequest = new GetImportRequest(document.Id);
        var stored = await Context.ExecuteAsync<GetImportRequest, Result<GetImportResponse, Error>>(_context, getRequest);
        Assert.True(stored.Success);
        Assert.Equal(5, stored.Value.Document.Activities.Count);

        var rowsRequest = new ListImportRowRequest(document.Id);
        var rows = await Context.ExecuteAsync<ListImportRowRequest, Result<ListImportRowResponse, Error>>(_context, rowsRequest);
        Assert.True(rows.Success);
        Assert.Equal(47, (int)rows.Value.TotalCount);
        Assert.Equal(47, rows.Value.Rows.Count);
        Assert.Equal(offset, rows.Value.Rows[0].Record.Periods[0].TimeZoneOffset);
    }

    [Fact]
    public async Task Upload_ShouldUseTimeZoneOffsetOfUserSettings_WhenOffsetIsNotPassed()
    {
        var response = await UploadSampleAsync(_context);

        Assert.True(response.Success);
        Assert.Equal(TimeZoneOffset.Default, response.Value.Document.TimeZoneOffset);
    }

    [Fact]
    public async Task Upload_ShouldReturnWarningsAndCounts_WhenSomeRowsAreSkipped()
    {
        const string text =
            "ID, Time item ID, Start Date, Start Time, End Date, End Time,  Duration, Duration in milliseconds, Activity ID, Activity Name, Notes, Category ID, Category Name\n" +
            "1, 1, 1 Oct 2026, 05:00, 1 Oct 2026, 06:00,  01:00:00, 1, 2, Work,  , , \n" +
            "2, 2, bad date, 05:00, 1 Oct 2026, 06:00,  01:00:00, 1, 2, Work,  , , \n";
        await using var content = new MemoryStream(Encoding.UTF8.GetBytes(text));
        var request = new UploadImportRequest(TimeTrackerKey, FileName, content);

        var response = await Context.ExecuteAsync<UploadImportRequest, Result<UploadImportResponse, Error>>(_context, request);

        Assert.True(response.Success);
        Assert.Equal(2, response.Value.Document.RowsInFile);
        Assert.Equal(1, response.Value.Document.RowsParsed);
        var warning = Assert.Single(response.Value.Warnings);
        Assert.Equal(3, warning.LineNumber);
    }

    [Fact]
    public async Task Upload_ShouldImportExportedNativeJson()
    {
        using var filler = Context.CreateExecutor();
        var tags = await filler.Tags.CreateManyAsync(_context, 2);
        var activities = await filler.Activities.CreateManyAsync(_context, 2);
        var now = DateTimeOffset.UtcNow;
        var period = new NewRecordPeriod(new DateTimePeriod(now.AddHours(-3), now.AddHours(-2)));
        var activityIds = new RelatedActivityIdsCollection(activities.Select(a => a.Id));
        var createRequest = new CreateRecordRequest([period], activityIds)
        {
            Tags = new RelatedTagIdsCollection([tags[0].Id])
        };
        var record = await filler.Records.CreateAsync(_context, createRequest);

        using var exported = new MemoryStream();
        var exportRequest = new ExportDataRequest(NativeJsonKey, _ => exported);
        await Context.ExecuteAsync<ExportDataRequest, Result<ExportDataResponse, Error>>(_context, exportRequest);
        exported.Position = 0;

        var uploadRequest = new UploadImportRequest(NativeJsonKey, FileName, exported);
        var response = await Context.ExecuteAsync<UploadImportRequest, Result<UploadImportResponse, Error>>(_context, uploadRequest);

        Assert.True(response.Success);
        var document = response.Value.Document;
        Assert.Equal(2, document.Tags.Count);
        Assert.Equal(2, document.Activities.Count);
        Assert.Equal(1, document.RowsInFile);
        var rowsRequest = new ListImportRowRequest(document.Id);
        var rows = await Context.ExecuteAsync<ListImportRowRequest, Result<ListImportRowResponse, Error>>(_context, rowsRequest);
        Assert.True(rows.Success);
        var row = Assert.Single(rows.Value.Rows);
        Assert.Equal(record.Id.ToString(), row.Record.Id.ToString());
        Assert.Equal([tags[0].Id.ToString()], row.Record.TagIds.Select(t => t.ToString()));
    }

    [Fact]
    public async Task Upload_ShouldReturnFormatNotFound_AndSaveNothing_WhenFormatIsUnknown()
    {
        await using var content = OpenSample();
        var request = new UploadImportRequest(FormatKey.Parse("unknown", null), FileName, content);

        var response = await Context.ExecuteAsync<UploadImportRequest, Result<UploadImportResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.FormatNotFound, response.Error.Type);
        var list = await ListAsync(_context);
        Assert.True(list.Success);
        Assert.Equal(0, (int)list.Value.TotalCount);
    }

    [Fact]
    public async Task Upload_ShouldReturnInvalidRequest_AndSaveNothing_WhenFileIsBroken()
    {
        await using var content = new MemoryStream(Encoding.UTF8.GetBytes("not a time tracker file"));
        var request = new UploadImportRequest(TimeTrackerKey, FileName, content);

        var response = await Context.ExecuteAsync<UploadImportRequest, Result<UploadImportResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.InvalidRequest, response.Error.Type);
        var list = await ListAsync(_context);
        Assert.True(list.Success);
        Assert.Equal(0, (int)list.Value.TotalCount);
    }

    [Fact]
    public async Task List_ShouldReturnOnlyImportsOfUser()
    {
        var other = await CreateOtherContextAsync();
        await UploadSampleAsync(_context);
        await UploadSampleAsync(_context);
        await UploadSampleAsync(other);

        var mine = await ListAsync(_context);
        var theirs = await ListAsync(other);

        Assert.True(mine.Success);
        Assert.True(theirs.Success);
        Assert.Equal(2, (int)mine.Value.TotalCount);
        Assert.Equal(1, (int)theirs.Value.TotalCount);
    }

    [Fact]
    public async Task ListRows_ShouldPage()
    {
        var response = await UploadSampleAsync(_context);
        Assert.True(response.Success);
        var request = new ListImportRowRequest(response.Value.Document.Id, new(10), new(40));

        var rows = await Context.ExecuteAsync<ListImportRowRequest, Result<ListImportRowResponse, Error>>(_context, request);

        Assert.True(rows.Success);
        Assert.Equal(47, (int)rows.Value.TotalCount);
        Assert.Equal(7, rows.Value.Rows.Count);
    }

    [Fact]
    public async Task Get_ListRows_Remove_ShouldNotAllowAccessToAnotherUsersImport()
    {
        var response = await UploadSampleAsync(_context);
        Assert.True(response.Success);
        var id = response.Value.Document.Id;
        var other = await CreateOtherContextAsync();

        var get = await Context.ExecuteAsync<GetImportRequest, Result<GetImportResponse, Error>>(other, new GetImportRequest(id));
        var rows = await Context.ExecuteAsync<ListImportRowRequest, Result<ListImportRowResponse, Error>>(other, new ListImportRowRequest(id));
        var remove = await Context.ExecuteAsync<RemoveImportRequest, Result<Error>>(other, new RemoveImportRequest(id));

        Assert.False(get.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, get.Error.Type);
        Assert.False(rows.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, rows.Error.Type);
        Assert.False(remove.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, remove.Error.Type);
        var mine = await ListAsync(_context);
        Assert.True(mine.Success);
        Assert.Equal(1, (int)mine.Value.TotalCount);
    }

    [Fact]
    public async Task Remove_ShouldDeleteImportWithRows()
    {
        var response = await UploadSampleAsync(_context);
        Assert.True(response.Success);
        var id = response.Value.Document.Id;

        var remove = await Context.ExecuteAsync<RemoveImportRequest, Result<Error>>(_context, new RemoveImportRequest(id));
        var get = await Context.ExecuteAsync<GetImportRequest, Result<GetImportResponse, Error>>(_context, new GetImportRequest(id));

        Assert.True(remove.Success);
        Assert.False(get.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, get.Error.Type);
    }
}

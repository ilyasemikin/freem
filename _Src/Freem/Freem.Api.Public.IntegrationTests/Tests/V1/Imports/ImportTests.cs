using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Freem.Domain;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Users.Models;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Imports;

public sealed class ImportTests : TestBase
{
    private static readonly FormatKey TimeTrackerKey = FormatKey.Parse("time-tracker", null);

    public ImportTests(TestContext context, ITestOutputHelper? output = null)
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();
    }

    private static Stream OpenSample()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "TimeTrackerSample.csv");
        return File.OpenRead(path);
    }

    private async Task<UploadImportResponse> UploadSampleAsync(TimeZoneOffset? offset = null)
    {
        await using var content = OpenSample();
        var response = await Context.Client.Imports.UploadAsync(TimeTrackerKey, "sample.csv", content, offset);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
        return response.Value;
    }

    [Fact]
    public async Task Upload_ShouldCreateImportWithRows()
    {
        var offset = new TimeZoneOffset(TimeSpan.FromHours(3));

        var uploaded = await UploadSampleAsync(offset);

        Assert.Equal(47, uploaded.Import.RowsInFile);
        Assert.Equal(47, uploaded.Import.RowsParsed);
        Assert.Equal(offset, uploaded.Import.TimeZoneOffset);
        Assert.Empty(uploaded.Warnings);

        var rows = await Context.Client.Imports.ListRowsAsync(uploaded.Import.Id, new ListImportRowRequest(0, 100));
        rows.ShouldSuccess();
        Assert.NotNull(rows.Value);
        Assert.Equal(47, rows.Value.Count);
    }

    [Fact]
    public async Task Upload_ShouldReturnImportInListAndGet()
    {
        var uploaded = await UploadSampleAsync();

        var list = await Context.Client.Imports.ListAsync(new ListImportRequest(0, 10));
        var get = await Context.Client.Imports.GetAsync(uploaded.Import.Id);

        list.ShouldSuccess();
        Assert.NotNull(list.Value);
        Assert.Equal(uploaded.Import.Id, Assert.Single(list.Value).Id);
        get.ShouldSuccess();
        Assert.NotNull(get.Value);
        Assert.Equal(47, get.Value.RowsParsed);
    }

    [Fact]
    public async Task ListActivities_ShouldPageActivitiesOfImport()
    {
        var uploaded = await UploadSampleAsync();

        var all = await Context.Client.Imports.ListActivitiesAsync(uploaded.Import.Id, new ListImportActivityRequest(0, 100));
        var page = await Context.Client.Imports.ListActivitiesAsync(uploaded.Import.Id, new ListImportActivityRequest(3, 10));

        all.ShouldSuccess();
        Assert.NotNull(all.Value);
        Assert.Equal(5, all.Value.Count);
        Assert.All(all.Value, a => Assert.Null(a.MappedActivityId));
        page.ShouldSuccess();
        Assert.NotNull(page.Value);
        Assert.Equal(2, page.Value.Count);
    }

    [Fact]
    public async Task ListTags_ShouldReturnEmpty_WhenFileHasNoTags()
    {
        var uploaded = await UploadSampleAsync();

        var tags = await Context.Client.Imports.ListTagsAsync(uploaded.Import.Id, new ListImportTagRequest(0, 100));

        tags.ShouldSuccess();
        Assert.NotNull(tags.Value);
        Assert.Empty(tags.Value);
    }

    [Fact]
    public async Task ListActivities_ShouldReturnNotFound_WhenImportDoesNotExist()
    {
        var activities = await Context.Client.Imports.ListActivitiesAsync(ImportDocumentId.Generate(), new ListImportActivityRequest(0, 100));
        var tags = await Context.Client.Imports.ListTagsAsync(ImportDocumentId.Generate(), new ListImportTagRequest(0, 100));

        activities.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        tags.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListRows_ShouldPage()
    {
        var uploaded = await UploadSampleAsync();

        var rows = await Context.Client.Imports.ListRowsAsync(uploaded.Import.Id, new ListImportRowRequest(40, 10));

        rows.ShouldSuccess();
        Assert.NotNull(rows.Value);
        Assert.Equal(7, rows.Value.Count);
    }

    [Fact]
    public async Task Remove_ShouldDeleteImport()
    {
        var uploaded = await UploadSampleAsync();

        var remove = await Context.Client.Imports.RemoveAsync(uploaded.Import.Id);
        var get = await Context.Client.Imports.GetAsync(uploaded.Import.Id);

        remove.ShouldSuccess();
        get.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenImportDoesNotExist()
    {
        var get = await Context.Client.Imports.GetAsync(ImportDocumentId.Generate());

        get.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Upload_ShouldReturnFormatNotFound_WhenFormatIsUnknown()
    {
        await using var content = OpenSample();

        var response = await Context.Client.Imports.UploadAsync(FormatKey.Parse("unknown", null), "sample.csv", content);

        response.ShouldHaveClientError(HttpStatusCode.NotFound, DomainErrorCatalog.Types.FormatNotFound);
    }

    [Fact]
    public async Task Upload_ShouldReturnInvalidRequest_WhenFileIsBroken()
    {
        await using var content = new MemoryStream(Encoding.UTF8.GetBytes("not a time tracker file"));

        var response = await Context.Client.Imports.UploadAsync(TimeTrackerKey, "broken.csv", content);

        response.ShouldHaveClientError(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }

    [Fact]
    public async Task Upload_ShouldReturnInvalidRequest_WhenTimeZoneOffsetIsInvalid()
    {
        using var client = Context.CreateHttpClient();
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("time-tracker"), "format");
        form.Add(new StringContent("not-an-offset"), "timeZoneOffset");
        var file = new StreamContent(OpenSample());
        file.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
        form.Add(file, "file", "sample.csv");

        using var response = await client.PostAsync("api/v1/imports", form);

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }

    [Fact]
    public async Task Upload_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        using var client = Context.CreateHttpClient();
        client.DefaultRequestHeaders.Authorization = null;
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("time-tracker"), "format");
        form.Add(new StreamContent(OpenSample()), "file", "sample.csv");

        using var response = await client.PostAsync("api/v1/imports", form);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

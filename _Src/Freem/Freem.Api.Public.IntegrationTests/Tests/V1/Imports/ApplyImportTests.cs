using Freem.Api.Public.Contracts.V1.Imports.Dtos;
using System.Net;
using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Imports;

public sealed class ApplyImportTests : TestBase
{
    private static readonly FormatKey TimeTrackerKey = FormatKey.Parse("time-tracker", null);

    public ApplyImportTests(TestContext context, ITestOutputHelper? output = null)
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();
    }

    private async Task<ImportDto> UploadSampleAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "TimeTrackerSample.csv");
        await using var content = File.OpenRead(path);
        var response = await Context.Client.Imports.UploadAsync(TimeTrackerKey, "sample.csv", content);

        response.ShouldSuccess();
        Assert.NotNull(response.Value);
        return response.Value.Import;
    }

    private async Task<IReadOnlyList<ImportActivityDto>> ListActivitiesAsync(ImportDocumentId importId)
    {
        var response = await Context.Client.Imports.ListActivitiesAsync(importId, new ImportActivityListRequest(0, 100));

        response.ShouldSuccess();
        Assert.NotNull(response.Value);
        return response.Value.Activities;
    }

    [Fact]
    public async Task Apply_ShouldCreateRecords_WhenAllActivitiesAreMapped()
    {
        var import = await UploadSampleAsync();
        var importActivities = await ListActivitiesAsync(import.Id);
        foreach (var activity in importActivities)
        {
            var activityId = await Context.Preparer.Activities.CreateAsync();
            var body = new ImportActivityMappingUpdateRequest(activityId);
            var map = await Context.Client.Imports.UpdateActivityMappingAsync(import.Id, activity.Id, body);
            map.ShouldSuccess();
        }

        var apply = await Context.Client.Imports.ApplyAsync(import.Id, new ImportApplyRequest(true, null));

        apply.ShouldSuccess();
        Assert.NotNull(apply.Value);
        Assert.Equal(47, apply.Value.RecordsCreated);
        Assert.Empty(apply.Value.SkippedRowIds);
        var records = await Context.Client.Records.ListAsync(new RecordListRequest(0, 100));
        records.ShouldSuccess();
        Assert.NotNull(records.Value);
        Assert.Equal(47, records.Value.Records.Count);
        var stored = await ListActivitiesAsync(import.Id);
        Assert.All(stored, a => Assert.NotNull(a.MappedActivityId));
    }

    [Fact]
    public async Task Apply_ShouldReturnUnprocessable_WhenActivitiesAreNotMapped()
    {
        var import = await UploadSampleAsync();

        var apply = await Context.Client.Imports.ApplyAsync(import.Id, new ImportApplyRequest(true, null));

        apply.ShouldHaveClientError(HttpStatusCode.UnprocessableEntity, DomainErrorCatalog.Types.ImportUnmappedEntities);
    }

    [Fact]
    public async Task Apply_ShouldReturnBadRequest_WhenSelectionIsAmbiguous()
    {
        var import = await UploadSampleAsync();
        ImportRowId[] rowIds = [ImportRowId.Generate()];

        var both = await Context.Client.Imports.ApplyAsync(import.Id, new ImportApplyRequest(true, rowIds));
        var none = await Context.Client.Imports.ApplyAsync(import.Id, new ImportApplyRequest(false, null));

        both.ShouldHaveClientError(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
        none.ShouldHaveClientError(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }

    [Fact]
    public async Task Apply_ShouldReturnNotFound_WhenImportDoesNotExist()
    {
        var apply = await Context.Client.Imports.ApplyAsync(ImportDocumentId.Generate(), new ImportApplyRequest(true, null));

        apply.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateActivityMapping_ShouldReturnNotFound_WhenActivityDoesNotExist()
    {
        var import = await UploadSampleAsync();
        var importActivities = await ListActivitiesAsync(import.Id);
        var importActivityId = importActivities[0].Id;
        var body = new ImportActivityMappingUpdateRequest(ActivityId.Generate());

        var response = await Context.Client.Imports.UpdateActivityMappingAsync(import.Id, importActivityId, body);

        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateActivityMapping_ShouldRemoveMapping_WhenActivityIdIsNull()
    {
        var import = await UploadSampleAsync();
        var importActivities = await ListActivitiesAsync(import.Id);
        var importActivityId = importActivities[0].Id;
        var activityId = await Context.Preparer.Activities.CreateAsync();
        await Context.Client.Imports.UpdateActivityMappingAsync(import.Id, importActivityId, new ImportActivityMappingUpdateRequest(activityId));

        var remove = await Context.Client.Imports.UpdateActivityMappingAsync(import.Id, importActivityId, new ImportActivityMappingUpdateRequest(null));

        remove.ShouldSuccess();
        var stored = await ListActivitiesAsync(import.Id);
        Assert.Null(stored.Single(a => a.Id.Equals(importActivityId)).MappedActivityId);
    }

    [Fact]
    public async Task Apply_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        using var client = Context.CreateHttpClient();
        client.DefaultRequestHeaders.Authorization = null;
        var importId = ImportDocumentId.Generate();
        var body = new { all = true };

        using var response = await client.PostAsJsonAsync($"api/v1/imports/{importId}/apply", body);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

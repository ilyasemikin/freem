using System.Net;
using System.Text.Json;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;
using Freem.Domain;
using Freem.Domain.Formats;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.DataTransfer;

public sealed class ExportDataTests : RecordTestBase
{
    private static readonly FormatKey NativeJsonKey = FormatKey.Parse("freem-json", null);

    public ExportDataTests(TestContext context, ITestOutputHelper? output = null)
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldReturnFileWithAllData()
    {
        var response = await Context.Client.DataTransfer.ExportAsync(NativeJsonKey);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);

        using var document = JsonDocument.Parse(response.Value);
        var root = document.RootElement;
        Assert.Equal("freem", root.GetProperty("format").GetString());
        Assert.Equal(1, root.GetProperty("version").GetInt32());
        Assert.Equal(1, root.GetProperty("tags").GetArrayLength());
        Assert.Equal(1, root.GetProperty("activities").GetArrayLength());
        Assert.Equal(AddedRecordIds.Count, root.GetProperty("records").GetArrayLength());
    }

    [Fact]
    public async Task Request_ShouldReturnFileHeaders()
    {
        using var client = Context.CreateHttpClient();

        using var response = await client.GetAsync("api/v1/export/freem-json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("attachment", response.Content.Headers.ContentDisposition?.DispositionType);
        Assert.EndsWith(".json", response.Content.Headers.ContentDisposition?.FileName);
    }

    [Fact]
    public async Task Request_ShouldReturnFormatNotFoundError_WhenFormatIsUnknown()
    {
        using var client = Context.CreateHttpClient();

        using var response = await client.GetAsync("api/v1/export/unknown");

        await response.ShouldHaveErrorAsync(HttpStatusCode.NotFound, DomainErrorCatalog.Types.FormatNotFound);
    }

    [Fact]
    public async Task Request_ShouldReturnInvalidRequestError_WhenFormatKeyIsMalformed()
    {
        using var client = Context.CreateHttpClient();

        using var response = await client.GetAsync("api/v1/export/Bad%20Key");

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }

    [Fact]
    public async Task Request_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        using var client = Context.CreateHttpClient();
        client.DefaultRequestHeaders.Authorization = null;

        using var response = await client.GetAsync("api/v1/export/freem-json");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

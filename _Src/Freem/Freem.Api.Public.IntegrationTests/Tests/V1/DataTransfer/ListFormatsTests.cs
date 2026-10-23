using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Freem.Domain.Formats;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.DataTransfer;

public sealed class ListFormatsTests : TestBase
{
    public ListFormatsTests(TestContext context, ITestOutputHelper? output = null)
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();
    }

    [Fact]
    public async Task Request_ShouldReturnNativeJsonForExportAndImport()
    {
        var key = FormatKey.Parse("freem-json", null);

        var response = await Context.Client.DataTransfer.ListFormatsAsync();

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
        Assert.Contains(response.Value.ExportFormats, format => format.Key == key);
        Assert.Contains(response.Value.ImportFormats, format => format.Key == key);
    }
}

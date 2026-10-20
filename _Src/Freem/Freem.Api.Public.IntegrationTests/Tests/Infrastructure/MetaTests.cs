using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.Infrastructure;

public sealed class MetaTests : TestBase
{
    public MetaTests(TestContext context, ITestOutputHelper output)
        : base(context, output)
    {
    }

    [Fact]
    public async Task Version_ShouldSuccess_WithoutAuthorization()
    {
        var response = await Context.Client.Meta.GetVersionAsync();

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
        Assert.False(string.IsNullOrEmpty(response.Value.Version));
    }
}

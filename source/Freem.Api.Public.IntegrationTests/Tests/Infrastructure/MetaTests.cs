using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.Infrastructure;

public sealed class MetaTests : TestBase
{
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

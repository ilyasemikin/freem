using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Tags.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Tags;

public sealed class ListTagTests : TagTestBase
{
    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new TagListRequest(0, 100);
        
        var response = await Context.Client.Tags.ListAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }
}
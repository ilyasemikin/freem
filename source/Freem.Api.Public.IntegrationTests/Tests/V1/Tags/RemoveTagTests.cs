using System.Net;
using Freem.Domain.Tags;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Tags.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Tags;

public sealed class RemoveTagTests : TagTestBase
{
    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Tags.RemoveAsync(AddedTagIds[0]);

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTagNotExists()
    {
        var tagId = TagId.Generate();
        
        var response = await Context.Client.Tags.RemoveAsync(tagId);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
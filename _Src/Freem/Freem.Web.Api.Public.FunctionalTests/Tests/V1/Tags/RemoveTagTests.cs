using System.Net;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags;

public sealed class RemoveTagTests : TagTestBase
{
    public RemoveTagTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Tags.RemoveAsync(AddedTagIds[0]);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTagNotExists()
    {
        var tagId = Guid.NewGuid().ToString();
        
        var response = await Context.Client.Tags.RemoveAsync(tagId);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
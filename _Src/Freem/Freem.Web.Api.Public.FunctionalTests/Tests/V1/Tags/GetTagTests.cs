using System.Net;
using Freem.Entities.Tags.Identifiers;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags;

public sealed class GetTagTests : TagTestBase
{
    public GetTagTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Tags.GetAsync(AddedTagIds[0]);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTagNotExists()
    {
        var tagId = Guid.NewGuid().ToString();
        
        var response = await Context.Client.Tags.GetAsync((TagIdentifier)tagId);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(response.Value);
    }
}
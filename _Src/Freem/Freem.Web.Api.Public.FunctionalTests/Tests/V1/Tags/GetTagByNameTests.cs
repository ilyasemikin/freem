using System.Net;
using Freem.Entities.Tags.Models;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags;

public sealed class GetTagByNameTests : TagTestBase
{
    public GetTagByNameTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = "name";
        Context.Preparer.Tags.Create(name);
        
        var response = await Context.Client.Tags.GetAsync((TagName)name);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTagNotExists()
    {
        var tagIdString = Guid.NewGuid().ToString();
        var name = new TagName(tagIdString);
        
        var response = await Context.Client.Tags.GetAsync(name);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(response.Value);
    }
}
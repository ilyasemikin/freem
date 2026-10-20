using System.Net;
using Freem.Domain.Tags.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Tags;

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
        await Context.Preparer.Tags.CreateAsync(name);
        
        var tagName = TagName.Parse(name, null);
        var response = await Context.Client.Tags.GetAsync(tagName);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTagNotExists()
    {
        var tagIdString = Guid.NewGuid().ToString();
        var name = TagName.Parse(tagIdString, null);
        
        var response = await Context.Client.Tags.GetAsync(name);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        Assert.Null(response.Value);
    }
}
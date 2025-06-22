using System.Net;
using Freem.Entities.Tags.Models;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags;

public sealed class CreateTagTests : TagTestBase
{
    public CreateTagTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = new TagName("name");
        var request = new CreateTagRequest(name);
        
        var response = await Context.Client.Tags.CreateAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNameAlreadyExists()
    {
        var name = new TagName("name");
        Context.Preparer.Tags.Create(name);
        
        var request = new CreateTagRequest(name);
        
        var response = await Context.Client.Tags.CreateAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
    }
}
using System.Net;
using Freem.Domain.Tags.Models;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Tags;

public sealed class CreateTagTests : TagTestBase
{
    public CreateTagTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = TagName.Parse("name", null);
        var request = new CreateTagRequest(name);
        
        var response = await Context.Client.Tags.CreateAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNameAlreadyExists()
    {
        var nameValue = "name";
        await Context.Preparer.Tags.CreateAsync(nameValue);
        
        var name = TagName.Parse(nameValue, null);
        
        var request = new CreateTagRequest(name);
        
        var response = await Context.Client.Tags.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }
}
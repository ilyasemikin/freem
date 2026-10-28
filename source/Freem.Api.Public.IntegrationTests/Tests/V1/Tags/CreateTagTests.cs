using System.Net;
using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Domain.Tags.Models;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Tags.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Tags;

public sealed class CreateTagTests : TagTestBase
{
    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = TagName.Parse("name", null);
        var request = new TagCreateRequest(name);
        
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
        
        var request = new TagCreateRequest(name);
        
        var response = await Context.Client.Tags.CreateAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }
}
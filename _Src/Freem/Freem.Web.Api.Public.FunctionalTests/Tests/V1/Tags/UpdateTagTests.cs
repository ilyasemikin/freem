using System.Net;
using Freem.Entities.Tags.Models;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Contracts.Models;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags;

public sealed class UpdateTagTests : TagTestBase
{
    public UpdateTagTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = new TagName("updated_name");
        var request = new UpdateTagRequest
        {
            Name = new UpdateField<TagName>(name)
        };

        var response = await Context.Client.Tags.UpdateAsync(AddedTagIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new UpdateTagRequest();
        
        var response = await Context.Client.Tags.UpdateAsync(AddedTagIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNameAlreadyExists()
    {
        var name = "name";
        Context.Preparer.Tags.Create(name);

        var request = new UpdateTagRequest
        {
            Name = new UpdateField<TagName>(name)
        };
        
        var response = await Context.Client.Tags.UpdateAsync(AddedTagIds[0], request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
    }
}
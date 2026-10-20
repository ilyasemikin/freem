using System.Net;
using Freem.Domain.Tags.Models;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.Contracts.Models;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Tags;

public sealed class UpdateTagTests : TagTestBase
{
    public UpdateTagTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = TagName.Parse("updated_name", null);
        var request = new UpdateTagRequest
        {
            Name = new UpdateField<TagName>(name)
        };

        var response = await Context.Client.Tags.UpdateAsync(AddedTagIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToDo()
    {
        var request = new UpdateTagRequest();
        
        var response = await Context.Client.Tags.UpdateAsync(AddedTagIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNameAlreadyExists()
    {
        var name = "name";
        await Context.Preparer.Tags.CreateAsync(name);

        var tagName = TagName.Parse(name, null);
        var request = new UpdateTagRequest
        {
            Name = new UpdateField<TagName>(tagName)
        };
        
        var response = await Context.Client.Tags.UpdateAsync(AddedTagIds[0], request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }
}
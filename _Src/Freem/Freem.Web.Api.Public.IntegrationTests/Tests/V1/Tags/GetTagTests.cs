using System.Net;
using Freem.Domain;
using Freem.Domain.Tags;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Tags.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Tags;

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
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTagNotExists()
    {
        var tagId = TagId.Generate();
        
        var response = await Context.Client.Tags.GetAsync(tagId);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        Assert.Null(response.Value);
    }

    [Fact]
    public async Task Request_ShouldReturnEntityNotFoundError_WhenTagNotExists()
    {
        var tagId = TagId.Generate();
        
        var response = await Context.Client.Tags.GetAsync(tagId);

        response.ShouldHaveClientError(HttpStatusCode.NotFound, DomainErrorCatalog.Types.EntityNotFound);
    }

    [Fact]
    public async Task Request_ShouldReturnInvalidRequestError_WhenTagIdIsMalformed()
    {
        using var client = Context.CreateHttpClient();

        // Ids accept any non-blank string, so only a blank value fails TagId.TryParse.
        using var response = await client.GetAsync("api/v1/tags/%20");

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }
}
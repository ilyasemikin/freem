using System.Net;
using Freem.Domain;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities;

public sealed class ListActivityTests : ActivityTestBase
{
    public ListActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new ListActivityRequest(0, 100);
        
        var response = await Context.Client.Activities.ListAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldReturnInvalidRequestError_WhenLimitIsNegative()
    {
        using var client = Context.CreateHttpClient();

        using var response = await client.GetAsync("api/v1/activities?offset=0&limit=-1");

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }
}
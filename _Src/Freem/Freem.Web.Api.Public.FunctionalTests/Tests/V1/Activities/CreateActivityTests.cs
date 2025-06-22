using System.Net;
using Freem.Entities.Activities.Models;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities;

public sealed class CreateActivityTests : ActivityTestBase
{
    public CreateActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var name = new ActivityName("name");
        var request = new CreateActivityRequest(name, AddedRelatedTags);
        
        var response = await Context.Client.Activities.CreateAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Value);
    }
}
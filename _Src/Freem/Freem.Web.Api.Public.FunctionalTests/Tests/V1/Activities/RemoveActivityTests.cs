using System.Net;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities;

public sealed class RemoveActivityTests : ActivityTestBase
{
    public RemoveActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var activityId = AddedActivityIds[0];
        var response = await Context.Client.Activities.RemoveAsync(activityId);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityDoesNotExist()
    {
        var activityId = Guid.NewGuid().ToString();
        
        var response = await Context.Client.Activities.RemoveAsync(activityId);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
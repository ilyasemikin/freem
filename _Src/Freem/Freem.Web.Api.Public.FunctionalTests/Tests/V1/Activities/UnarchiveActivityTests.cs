using System.Net;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities;

public sealed class UnarchiveActivityTests : ActivityTestBase
{
    public UnarchiveActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var activityId = AddedActivityIds[0];
        Context.Preparer.Activities.Archive(activityId);
        
        var response = await Context.Client.Activities.UnarchiveAsync(activityId);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityHasInvalidStatus()
    {
        var activityId = AddedActivityIds[0];
        var response = await Context.Client.Activities.UnarchiveAsync(activityId);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityNotExists()
    {
        var activityId = Guid.NewGuid().ToString();
        
        var response = await Context.Client.Activities.UnarchiveAsync(activityId);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
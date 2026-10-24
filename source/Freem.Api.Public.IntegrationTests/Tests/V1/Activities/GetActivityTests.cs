using System.Net;
using Freem.Domain.Activities;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Activities;

public sealed class GetActivityTests : ActivityTestBase
{
    public GetActivityTests(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Activities.GetAsync(AddedActivityIds[0]);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityNotExists()
    {
        var activityId = ActivityId.Generate();
        
        var response = await Context.Client.Activities.GetAsync(activityId);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
        Assert.Null(response.Value);
    }
}
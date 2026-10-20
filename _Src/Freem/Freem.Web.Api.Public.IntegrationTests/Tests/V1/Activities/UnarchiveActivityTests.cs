using System.Net;
using Freem.Domain.Activities;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities;

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
        await Context.Preparer.Activities.ArchiveAsync(activityId);
        
        var response = await Context.Client.Activities.UnarchiveAsync(activityId);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityHasInvalidStatus()
    {
        var activityId = AddedActivityIds[0];
        var response = await Context.Client.Activities.UnarchiveAsync(activityId);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenActivityNotExists()
    {
        var activityId = ActivityId.Generate();
        
        var response = await Context.Client.Activities.UnarchiveAsync(activityId);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
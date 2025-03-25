using System.Net;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running;

public sealed class RemoveRunningRecordTests : RunningRecordTestBase
{
    public RemoveRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        Context.Preparer.Records.StartRunning(null, AddedRelatedActivities);
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Records.RemoveRunningAsync();

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordIsNotFound()
    {
        Context.Preparer.Records.StopRunning();

        var response = await Context.Client.Records.RemoveRunningAsync();
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
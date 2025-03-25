using System.Net;
using Freem.Web.Api.Public.Contracts.Records.Running;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running;

public sealed class StartRunningRecordTests : RunningRecordTestBase
{
    public StartRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var startAt = DateTimeOffset.UtcNow;

        var request = new StartRunningRecordRequest(startAt, AddedRelatedActivities, AddedRelatedTags);

        var response = await Context.Client.Records.StartAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenAnotherExists()
    {
        var startAt = DateTimeOffset.UtcNow;
        Context.Preparer.Records.StartRunning(startAt, AddedRelatedActivities);
        
        startAt = DateTimeOffset.UtcNow;
        var request = new StartRunningRecordRequest(startAt, AddedRelatedActivities, AddedRelatedTags);
        
        var response = await Context.Client.Records.StartAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
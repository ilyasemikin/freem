using System.Net;
using Freem.Web.Api.Public.Contracts.DTO.Records.Running;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running;

public sealed class StopRunningRecordTests : RunningRecordTestBase
{
    public StopRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        Context.Preparer.Records.StartRunning(null, AddedRelatedActivities);
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var endAt = DateTimeOffset.UtcNow;
        var request = new StopRunningRecordRequest(endAt);

        var response = await Context.Client.Records.StopAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenEndAtIsNull()
    {
        var request = new StopRunningRecordRequest();
        
        var response = await Context.Client.Records.StopAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenEndAtIsBefore()
    {
        var endAt = DateTimeOffset.UtcNow.AddHours(-1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Context.Client.Records.StopAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToStop()
    {
        Context.Preparer.Records.StopRunning();

        var request = new StopRunningRecordRequest();
        var response = await Context.Client.Records.StopAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
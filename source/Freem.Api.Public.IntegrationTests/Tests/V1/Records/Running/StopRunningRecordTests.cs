using System.Net;
using Freem.Api.Public.Contracts.V1.Records.Running;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running;

public sealed class StopRunningRecordTests : RunningRecordTestBase
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Records.StartRunningAsync(null, AddedRelatedActivities);
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var endAt = DateTimeOffset.UtcNow;
        var request = new RunningRecordStopRequest(endAt);

        var response = await Context.Client.Records.StopAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenEndAtIsNull()
    {
        var request = new RunningRecordStopRequest();
        
        var response = await Context.Client.Records.StopAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenEndAtIsBefore()
    {
        var endAt = DateTimeOffset.UtcNow.AddHours(-1);
        var request = new RunningRecordStopRequest(endAt);
        
        var response = await Context.Client.Records.StopAsync(request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToStop()
    {
        await Context.Preparer.Records.StopRunningAsync();

        var request = new RunningRecordStopRequest();
        var response = await Context.Client.Records.StopAsync(request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
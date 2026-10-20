using System.Net;
using Freem.Api.Public.Contracts.DTO.Records.Running;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running;

public sealed class StopRunningRecordTests : RunningRecordTestBase
{
    public StopRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Records.StartRunningAsync(null, AddedRelatedActivities);
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var endAt = DateTimeOffset.UtcNow;
        var request = new StopRunningRecordRequest(endAt);

        var response = await Context.Client.Records.StopAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenEndAtIsNull()
    {
        var request = new StopRunningRecordRequest();
        
        var response = await Context.Client.Records.StopAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenEndAtIsBefore()
    {
        var endAt = DateTimeOffset.UtcNow.AddHours(-1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Context.Client.Records.StopAsync(request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.UnprocessableContent);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenNothingToStop()
    {
        await Context.Preparer.Records.StopRunningAsync();

        var request = new StopRunningRecordRequest();
        var response = await Context.Client.Records.StopAsync(request);
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
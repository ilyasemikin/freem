using Freem.Web.Api.Public.Contracts.DTO.Records.Running;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Running;

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
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldSuccess_WhenAnotherExists()
    {
        var startAt = DateTimeOffset.UtcNow;
        await Context.Preparer.Records.StartRunningAsync(startAt, AddedRelatedActivities);
        
        startAt = DateTimeOffset.UtcNow;
        var request = new StartRunningRecordRequest(startAt, AddedRelatedActivities, AddedRelatedTags);
        
        var response = await Context.Client.Records.StartAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }
}
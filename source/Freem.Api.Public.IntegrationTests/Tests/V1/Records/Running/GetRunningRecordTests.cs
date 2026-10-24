using System.Net;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running;

public sealed class GetRunningRecordTests : RunningRecordTestBase
{
    public GetRunningRecordTests(TestContext context, ITestOutputHelper? output = null) 
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
        var response = await Context.Client.Records.GetRunningAsync();
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordNotExists()
    {
        await Context.Preparer.Records.StopRunningAsync();

        var response = await Context.Client.Records.GetRunningAsync();
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
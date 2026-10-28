using System.Net;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running;

public sealed class RemoveRunningRecordTests : RunningRecordTestBase
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Records.StartRunningAsync(null, AddedRelatedActivities);
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var response = await Context.Client.Records.RemoveRunningAsync();

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenRecordIsNotFound()
    {
        await Context.Preparer.Records.StopRunningAsync();

        var response = await Context.Client.Records.RemoveRunningAsync();
        
        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
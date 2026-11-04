using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running;

public sealed class GetRunningRecordTests : RunningRecordTestBase
{
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
        Assert.NotNull(response.Unwrap().RunningRecord);
    }

    [Fact]
    public async Task Request_ShouldReturnNull_WhenRecordNotExists()
    {
        await Context.Preparer.Records.StopRunningAsync();

        var response = await Context.Client.Records.GetRunningAsync();
        
        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.Null(response.Unwrap().RunningRecord);
    }
}
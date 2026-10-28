using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.Infrastructure;

public sealed class HealthTests : TestBase
{
    [Fact]
    public async Task Liveness_ShouldSuccess_WithoutAuthorization()
    {
        var response = await Context.Client.Health.GetLivenessAsync();

        Assert.NotNull(response);
        response.ShouldSuccess();
    }
}

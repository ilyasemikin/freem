using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.Infrastructure;

public sealed class HealthTests : TestBase
{
    public HealthTests(TestContext context, ITestOutputHelper output)
        : base(context, output)
    {
    }

    [Fact]
    public async Task Liveness_ShouldSuccess_WithoutAuthorization()
    {
        var response = await Context.Client.Health.GetLivenessAsync();

        Assert.NotNull(response);
        response.ShouldSuccess();
    }
}

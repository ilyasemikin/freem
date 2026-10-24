using Freem.Api.Public.Contracts.V1.Statistics;
using Freem.Time.Extensions;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Statistics.Base;
using Freem.Time;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Statistics;

public sealed class StatisticsPerDaysTests : StatisticsTestBase
{
    public StatisticsPerDaysTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var startAt = DateTimeOffset.UtcNow.AddDays(-1).ToDateOnly();
        var period = new DatePeriod(startAt, startAt.AddDays(1));
        var request = new StatisticsPerDaysRequest(period);

        var response = await Context.Client.Statistics.GetAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }
}
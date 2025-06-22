using System.Net;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Statistics.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Statistics;

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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
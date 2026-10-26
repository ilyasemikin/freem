using Freem.Api.Public.Contracts.V1.Statistics;
using Freem.Api.Public.Contracts.V1.Statistics.Periods;
using Freem.Time.Extensions;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Statistics.Base;
using Freem.Time;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Statistics;

public sealed class StatisticsPerPeriodTests : StatisticsTestBase
{
    public StatisticsPerPeriodTests(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    [Fact]
    public async Task Day_ShouldSuccess()
    {
        var day = DateTimeOffset.UtcNow.AddDays(-1).ToDateOnly();
        var period = new DayUnitPeriod(day);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.Client.Statistics.GetPerPeriodAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Month_ShouldSuccess()
    {
        var month = new MonthOnly(Month.March, 2025);
        var period = new MonthUnitPeriod(month);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.Client.Statistics.GetPerPeriodAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Year_ShouldSuccess()
    {
        var period = new YearUnitPeriod(2025);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.Client.Statistics.GetPerPeriodAsync(request);
        
        Assert.NotNull(response);
        response.ShouldSuccess();
    }
}
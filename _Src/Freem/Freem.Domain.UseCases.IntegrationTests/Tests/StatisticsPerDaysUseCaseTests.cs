using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Statistics.PerDays;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class StatisticsPerDaysUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;

    private HandlerContext _context = null!;
    
    public StatisticsPerDaysUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        var activities = await filler.Activities.CreateManyAsync(_context, 2);

        await filler.Records.CreateManyAsync(_context, [activities[0].Id], RecordsCount);
        await filler.Records.CreateManyAsync(_context, [activities[1].Id], RecordsCount);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var now = DateOnly.FromDateTime(DateTime.Now);
        var day = now.AddDays(-1);
        var period = new DatePeriod(day, day);

        var request = new StatisticsPerDaysRequest(period);
        var response = await Context.ExecuteAsync<StatisticsPerDaysRequest, Result<StatisticsPerDaysResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Statistics);
        Assert.Null(response.Error);
        
        Assert.Single(response.Value.Statistics);
        
        var (date, statistics) = response.Value.Statistics.Single();
        Assert.Equal(now.AddDays(-1), date);
        Assert.Equal(2, statistics.PerActivities.Count);
    }
    
}
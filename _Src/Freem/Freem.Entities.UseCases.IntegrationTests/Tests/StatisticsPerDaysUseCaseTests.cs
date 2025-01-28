using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class StatisticsPerDaysUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;

    private readonly UseCaseExecutionContext _context;
    
    public StatisticsPerDaysUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);

        var activities = filler.Activities
            .CreateMany(_context, 2)
            .ToArray();

        filler.Records.CreateMany(_context, [activities[0].Id], RecordsCount);
        filler.Records.CreateMany(_context, [activities[1].Id], RecordsCount);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var now = DateOnly.FromDateTime(DateTime.Now);
        var period = new DatePeriod(now, now.AddDays(1));

        var request = new StatisticsPerDaysRequest(period);
        var response = await Context.ExecuteAsync<StatisticsPerDaysRequest, StatisticsPerDaysResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Statistics);
        Assert.Null(response.Error);
        
        Assert.Single(response.Statistics);
        
        var (date, statistics) = response.Statistics.Single();
        Assert.Equal(now, date);
        Assert.Equal(2, statistics.ActivitiesStatistics.ActivitiesCount);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var now = DateOnly.FromDateTime(DateTime.Now);
        var period = new DatePeriod(now, now.AddDays(1));
        var request = new StatisticsPerDaysRequest(period);

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<StatisticsPerDaysRequest, StatisticsPerDaysResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
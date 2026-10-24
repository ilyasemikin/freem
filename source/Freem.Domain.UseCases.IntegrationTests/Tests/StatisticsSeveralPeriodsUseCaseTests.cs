using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Statistics.PerDays;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class StatisticsSeveralPeriodsUseCaseTests : UseCaseTestBase
{
    private static readonly DateOnly Day = new(2025, 3, 10);

    private HandlerContext _context = null!;

    public StatisticsSeveralPeriodsUseCaseTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        var activity = await filler.Activities.CreateAsync(_context);
        var activities = new RelatedActivityIdsCollection([activity.Id]);

        var dayStart = new DateTimeOffset(Day.Year, Day.Month, Day.Day, 0, 0, 0, TimeSpan.Zero);
        var periods = new[]
        {
            new NewRecordPeriod(new DateTimePeriod(dayStart.AddHours(10), dayStart.AddHours(11))),
            new NewRecordPeriod(new DateTimePeriod(dayStart.AddHours(14), dayStart.AddHours(16))),
            new NewRecordPeriod(new DateTimePeriod(dayStart.AddHours(23), dayStart.AddHours(26)))
        };
        await filler.Records.CreateAsync(_context, new CreateRecordRequest(periods, activities));

        var overlappingPeriod = new DateTimePeriod(dayStart.AddHours(10).AddMinutes(30), dayStart.AddHours(12));
        await filler.Records.CreateAsync(_context, new CreateRecordRequest(overlappingPeriod, activities));
    }

    [Fact]
    public async Task PerDays_ShouldCountEachPeriodOnceAndClipByDay()
    {
        var request = new StatisticsPerDaysRequest(new DatePeriod(Day, Day.AddDays(1)));

        var response = await Context.ExecuteAsync<StatisticsPerDaysRequest, Result<StatisticsPerDaysResponse, Error>>(_context, request);

        Assert.True(response.Success);

        // День 1: 10:00-12:00 (периоды двух записей пересекаются, считаются один раз), 14:00-16:00 и 23:00-24:00
        Assert.Equal(TimeSpan.FromHours(5), response.Value.Statistics[Day].RecordedTime);
        // День 2: хвост третьего периода первой записи 00:00-02:00
        Assert.Equal(TimeSpan.FromHours(2), response.Value.Statistics[Day.AddDays(1)].RecordedTime);
    }
}

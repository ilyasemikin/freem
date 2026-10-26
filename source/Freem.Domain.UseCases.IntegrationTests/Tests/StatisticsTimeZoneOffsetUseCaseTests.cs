using Freem.Domain.Relations.Collections;
using Freem.Domain.Users.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class StatisticsTimeZoneOffsetUseCaseTests : UseCaseTestBase
{
    private static readonly DateOnly Day = new(2025, 3, 10);

    public StatisticsTimeZoneOffsetUseCaseTests(TestContext context)
        : base(context)
    {
    }

    // Записи по UTC: D-1 22:00-23:00, D 04:00-06:00, D 20:00-22:00
    [Theory]
    [InlineData(0, 4)]
    [InlineData(180, 4)]
    [InlineData(-300, 3)]
    [InlineData(-720, 2)]
    public async Task PerPeriod_ShouldUseUserDayBounds(int offsetMinutes, int expectedHours)
    {
        var context = await CreateUserWithRecordsAsync(offsetMinutes);
        var period = new DayStatisticsPeriod(Day);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.ExecuteAsync<StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>>(context, request);

        Assert.True(response.Success);
        Assert.Equal(TimeSpan.FromHours(expectedHours), response.Value.Statistics.RecordedTime);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(180, 1)]
    [InlineData(-300, 0)]
    public async Task PerPeriod_ShouldUseUserBoundsOfNextDay(int offsetMinutes, int expectedHours)
    {
        var context = await CreateUserWithRecordsAsync(offsetMinutes);
        var day = Day.AddDays(1);
        var period = new DayStatisticsPeriod(day);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.ExecuteAsync<StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>>(context, request);

        Assert.True(response.Success);
        Assert.Equal(TimeSpan.FromHours(expectedHours), response.Value.Statistics.RecordedTime);
    }

    private async Task<HandlerContext> CreateUserWithRecordsAsync(int offsetMinutes)
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        var context = new HandlerContext(userId);

        var value = TimeSpan.FromMinutes(offsetMinutes);
        var offset = new TimeZoneOffset(value);
        await filler.UsersSettings.UpdateTimeZoneOffsetAsync(context, offset);

        var activity = await filler.Activities.CreateAsync(context);
        var activities = new RelatedActivityIdsCollection([activity.Id]);

        var dayStart = new DateTimeOffset(Day.Year, Day.Month, Day.Day, 0, 0, 0, TimeSpan.Zero);
        var periods = new[]
        {
            new DateTimePeriod(dayStart.AddHours(-2), dayStart.AddHours(-1)),
            new DateTimePeriod(dayStart.AddHours(4), dayStart.AddHours(6)),
            new DateTimePeriod(dayStart.AddHours(20), dayStart.AddHours(22))
        };
        foreach (var period in periods)
            await filler.Records.CreateAsync(context, new CreateRecordRequest(period, activities));

        return context;
    }
}

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

public sealed class StatisticsWeekUseCaseTests : UseCaseTestBase
{
    // Понедельник
    private static readonly DateOnly Monday = new(2026, 10, 26);

    public StatisticsWeekUseCaseTests(TestContext context)
        : base(context)
    {
    }

    // Записи по UTC: воскресенье до недели 1 ч, понедельник 2 ч, воскресенье в конце недели 3 ч, следующий понедельник 4 ч
    [Theory]
    [InlineData(WeekStart.Value.Monday, "2026-10-26", 5)]
    [InlineData(WeekStart.Value.Monday, "2026-11-01", 5)]
    [InlineData(WeekStart.Value.Sunday, "2026-10-26", 3)]
    [InlineData(WeekStart.Value.Sunday, "2026-11-01", 7)]
    public async Task PerPeriod_ShouldUseUserWeekStart(WeekStart.Value weekStart, string day, int expectedHours)
    {
        var settingsWeekStart = new WeekStart(weekStart);
        var context = await CreateUserWithRecordsAsync(settingsWeekStart, TimeZoneOffset.Default);
        var date = DateOnly.Parse(day);
        var period = new WeekStatisticsPeriod(date);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.ExecuteAsync<StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>>(context, request);

        Assert.True(response.Success);
        Assert.Equal(TimeSpan.FromHours(expectedHours), response.Value.Statistics.RecordedTime);
    }

    [Fact]
    public async Task PerPeriod_ShouldUseUserDayBounds()
    {
        // При +03:00 неделя с понедельника начинается в воскресенье 21:00 UTC: в неё попадает час воскресенья 22:00-23:00
        var offsetValue = TimeSpan.FromHours(3);
        var offset = new TimeZoneOffset(offsetValue);
        var context = await CreateUserWithRecordsAsync(WeekStart.Monday, offset);
        var period = new WeekStatisticsPeriod(Monday);
        var request = new StatisticsPerPeriodRequest(period);

        var response = await Context.ExecuteAsync<StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>>(context, request);

        Assert.True(response.Success);
        Assert.Equal(TimeSpan.FromHours(6), response.Value.Statistics.RecordedTime);
    }

    private async Task<HandlerContext> CreateUserWithRecordsAsync(WeekStart weekStart, TimeZoneOffset offset)
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        var context = new HandlerContext(userId);

        await filler.UsersSettings.UpdateWeekStartAsync(context, weekStart);
        await filler.UsersSettings.UpdateTimeZoneOffsetAsync(context, offset);

        var activity = await filler.Activities.CreateAsync(context);
        var activities = new RelatedActivityIdsCollection([activity.Id]);

        var mondayStart = new DateTimeOffset(Monday.Year, Monday.Month, Monday.Day, 0, 0, 0, TimeSpan.Zero);
        var periods = new[]
        {
            new DateTimePeriod(mondayStart.AddHours(-2), mondayStart.AddHours(-1)),
            new DateTimePeriod(mondayStart.AddHours(10), mondayStart.AddHours(12)),
            new DateTimePeriod(mondayStart.AddDays(6).AddHours(10), mondayStart.AddDays(6).AddHours(13)),
            new DateTimePeriod(mondayStart.AddDays(7).AddHours(10), mondayStart.AddDays(7).AddHours(14))
        };
        foreach (var period in periods)
            await filler.Records.CreateAsync(context, new CreateRecordRequest(period, activities));

        return context;
    }
}

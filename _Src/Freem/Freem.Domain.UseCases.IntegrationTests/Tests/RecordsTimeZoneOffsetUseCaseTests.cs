using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Records.List;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RecordsTimeZoneOffsetUseCaseTests : UseCaseTestBase
{
    private static readonly DateTimeOffset StartAt = new(2025, 3, 10, 10, 0, 0, TimeSpan.Zero);
    private static readonly TimeZoneOffset FirstOffset = new(TimeSpan.FromHours(3));
    private static readonly TimeZoneOffset SecondOffset = new(TimeSpan.FromMinutes(-330));

    private HandlerContext _context = null!;
    private ActivityId _activityId = null!;

    public RecordsTimeZoneOffsetUseCaseTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        var activity = await filler.Activities.CreateAsync(_context);
        _activityId = activity.Id;

        await filler.UsersSettings.UpdateTimeZoneOffsetAsync(_context, FirstOffset);
    }

    [Fact]
    public async Task Create_ShouldSetCurrentOffsetToAllPeriods()
    {
        var recordId = await CreateRecordAsync();

        var record = await GetRecordAsync(recordId);
        Assert.Equal(2, record.Periods.Count);
        Assert.All(record.Periods, p => Assert.Equal(FirstOffset, p.TimeZoneOffset));
    }

    [Fact]
    public async Task AddPeriod_ShouldSetCurrentOffset()
    {
        var recordId = await CreateRecordAsync();
        await UpdateOffsetAsync(SecondOffset);

        var period = new DateTimePeriod(StartAt.AddHours(5), StartAt.AddHours(6));
        var request = new AddRecordPeriodRequest(recordId, period);
        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);
        Assert.True(response.Success);

        var record = await GetRecordAsync(recordId);
        var added = record.Periods.Single(p => p.Id.Equals(response.Value.Id));
        Assert.Equal(SecondOffset, added.TimeZoneOffset);
        Assert.All(record.Periods.Where(p => !p.Id.Equals(added.Id)), p => Assert.Equal(FirstOffset, p.TimeZoneOffset));
    }

    [Fact]
    public async Task UpdatePeriod_ShouldSetCurrentOffset_WhenTimeChanges()
    {
        var recordId = await CreateRecordAsync();
        var periodId = (await GetRecordAsync(recordId)).Periods[0].Id;
        await UpdateOffsetAsync(SecondOffset);

        var period = new DateTimePeriod(StartAt, StartAt.AddMinutes(30));
        var request = new UpdateRecordPeriodRequest(recordId, periodId)
        {
            Period = new UpdateField<DateTimePeriod>(period)
        };
        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);
        Assert.True(response.Success);

        var record = await GetRecordAsync(recordId);
        Assert.Equal(SecondOffset, record.Periods.Single(p => p.Id.Equals(periodId)).TimeZoneOffset);
    }

    [Fact]
    public async Task UpdatePeriod_ShouldKeepOffset_WhenOnlyDescriptionChanges()
    {
        var recordId = await CreateRecordAsync();
        var periodId = (await GetRecordAsync(recordId)).Periods[0].Id;
        await UpdateOffsetAsync(SecondOffset);

        var description = RecordPeriodDescription.Parse("description", null);
        var request = new UpdateRecordPeriodRequest(recordId, periodId)
        {
            Description = new UpdateField<RecordPeriodDescription?>(description)
        };
        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);
        Assert.True(response.Success);

        var record = await GetRecordAsync(recordId);
        Assert.Equal(FirstOffset, record.Periods.Single(p => p.Id.Equals(periodId)).TimeZoneOffset);
    }

    [Fact]
    public async Task StopRunningRecord_ShouldSetCurrentOffset()
    {
        using (var filler = Context.CreateExecutor())
            await filler.RunningRecords.StartAsync(_context, [_activityId]);
        await UpdateOffsetAsync(SecondOffset);

        var endAt = DateTimeOffset.UtcNow.AddHours(1);
        var request = new StopRunningRecordRequest(endAt);
        var response = await Context.ExecuteAsync<StopRunningRecordRequest, Result<Error>>(_context, request);
        Assert.True(response.Success);

        var listRequest = new ListRecordRequest();
        var listResponse = await Context.ExecuteAsync<ListRecordRequest, Result<ListRecordResponse, Error>>(_context, listRequest);
        Assert.True(listResponse.Success);
        var record = Assert.Single(listResponse.Value.Records);
        var period = Assert.Single(record.Periods);
        Assert.Equal(SecondOffset, period.TimeZoneOffset);
    }

    private async Task<RecordId> CreateRecordAsync()
    {
        using var filler = Context.CreateExecutor();

        var activities = new RelatedActivityIdsCollection([_activityId]);
        var periods = new[]
        {
            new NewRecordPeriod(new DateTimePeriod(StartAt, StartAt.AddHours(1))),
            new NewRecordPeriod(new DateTimePeriod(StartAt.AddHours(2), StartAt.AddHours(3)))
        };
        var record = await filler.Records.CreateAsync(_context, new CreateRecordRequest(periods, activities));
        return record.Id;
    }

    private async Task<Domain.Entities.Records.Record> GetRecordAsync(RecordId id)
    {
        using var executor = Context.CreateExecutor();
        return await executor.Records.RequiredGetAsync(_context, id);
    }

    private async Task UpdateOffsetAsync(TimeZoneOffset offset)
    {
        using var executor = Context.CreateExecutor();
        await executor.UsersSettings.UpdateTimeZoneOffsetAsync(_context, offset);
    }
}

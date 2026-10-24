using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;
using Record = Freem.Domain.Entities.Records.Record;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RecordsAddPeriodUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private Record _record = null!;

    public RecordsAddPeriodUseCaseTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        var activity = await filler.Activities.CreateAsync(_context);
        _record = await filler.Records.CreateAsync(_context, [activity.Id]);
    }

    private DateTimePeriod CreateAfterRecord(int startHours, int durationHours)
    {
        var startAt = _record.Periods.Last().Period.EndAt.AddHours(startHours);
        return new DateTimePeriod(startAt, startAt.AddHours(durationHours));
    }

    private async Task<Record> GetRecordAsync()
    {
        using var executor = Context.CreateExecutor();
        return await executor.Records.RequiredGetAsync(_context, _record.Id);
    }

    private async Task<RecordPeriodId> AddPeriodAsync(DateTimePeriod period)
    {
        var request = new AddRecordPeriodRequest(_record.Id, period);
        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);
        Assert.True(response.Success);
        return response.Value.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var period = CreateAfterRecord(1, 2);
        var description = RecordPeriodDescription.Parse("afternoon", null);
        var request = new AddRecordPeriodRequest(_record.Id, period) { Description = description };

        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);

        var actual = await GetRecordAsync();
        Assert.Equal(2, actual.Periods.Count);
        Assert.Equal(response.Value.Id, actual.Periods[1].Id);
        Assert.Equal(period, actual.Periods[1].Period);
        Assert.Equal(description, actual.Periods[1].Description);
    }

    [Fact]
    public async Task ShouldPublishRecordUpdatedEventWithPeriodsSnapshot()
    {
        var periodId = await AddPeriodAsync(CreateAfterRecord(1, 1));

        var request = new ListEventRequest { EntityType = EntityType.Record };
        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, request);

        Assert.True(response.Success);
        var updated = Assert.Single(response.Value.Events, e => e.Type.Equals(EventTypes.Records.Updated));
        var periods = updated.Payload.GetRequired(nameof(RecordUpdatedEvent.Periods));
        Assert.Contains(_record.Periods[0].Id.ToString(), periods);
        Assert.Contains(periodId.ToString(), periods);
    }

    [Fact]
    public async Task ShouldSuccess_WhenPeriodTouchesAnother()
    {
        var period = CreateAfterRecord(0, 1);
        var request = new AddRecordPeriodRequest(_record.Id, period);

        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);

        Assert.True(response.Success);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodOverlapsAnother()
    {
        var existing = _record.Periods[0].Period;
        var period = new DateTimePeriod(existing.EndAt.AddMinutes(-1), existing.EndAt.AddHours(1));
        var request = new AddRecordPeriodRequest(_record.Id, period);

        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);

        var actual = await GetRecordAsync();
        Assert.Single(actual.Periods);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodHasZeroDuration()
    {
        var startAt = _record.Periods.Last().Period.EndAt.AddHours(1);
        var request = new AddRecordPeriodRequest(_record.Id, new DateTimePeriod(startAt, startAt));

        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var request = new AddRecordPeriodRequest(RecordId.Generate(), CreateAfterRecord(1, 1));

        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordBelongsToAnotherUser()
    {
        using var filler = Context.CreateExecutor();
        var anotherUserId = await filler.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var request = new AddRecordPeriodRequest(_record.Id, CreateAfterRecord(1, 1));

        var response = await Context.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(anotherContext, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
}

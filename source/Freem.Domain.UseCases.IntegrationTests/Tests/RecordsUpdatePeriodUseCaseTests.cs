using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;
using Record = Freem.Domain.Entities.Records.Record;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RecordsUpdatePeriodUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private Record _record = null!;

    public RecordsUpdatePeriodUseCaseTests(TestContext context)
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
    public async Task ShouldSuccess_WhenUpdatePeriod()
    {
        var current = _record.Periods[0];
        var period = new DateTimePeriod(current.Period.StartAt, current.Period.EndAt.AddHours(-1));
        var request = new UpdateRecordPeriodRequest(_record.Id, current.Id)
        {
            Period = new UpdateField<DateTimePeriod>(period)
        };

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.True(response.Success);

        var actual = await GetRecordAsync();
        var actualPeriod = Assert.Single(actual.Periods);
        Assert.Equal(current.Id, actualPeriod.Id);
        Assert.Equal(period, actualPeriod.Period);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateAndClearDescription()
    {
        var current = _record.Periods[0];
        var description = RecordPeriodDescription.Parse("description", null);
        var setRequest = new UpdateRecordPeriodRequest(_record.Id, current.Id)
        {
            Description = new UpdateField<RecordPeriodDescription?>(description)
        };

        var setResponse = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, setRequest);
        Assert.True(setResponse.Success);
        Assert.Equal(description, (await GetRecordAsync()).Periods[0].Description);

        var clearRequest = new UpdateRecordPeriodRequest(_record.Id, current.Id)
        {
            Description = new UpdateField<RecordPeriodDescription?>(null)
        };

        var clearResponse = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, clearRequest);
        Assert.True(clearResponse.Success);
        Assert.Null((await GetRecordAsync()).Periods[0].Description);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateRecordPeriodRequest(_record.Id, _record.Periods[0].Id);

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.NothingToDo, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodDoesNotExist()
    {
        var request = new UpdateRecordPeriodRequest(_record.Id, RecordPeriodId.Generate())
        {
            Period = new UpdateField<DateTimePeriod>(CreateAfterRecord(1, 1))
        };

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var request = new UpdateRecordPeriodRequest(RecordId.Generate(), _record.Periods[0].Id)
        {
            Period = new UpdateField<DateTimePeriod>(CreateAfterRecord(1, 1))
        };

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordBelongsToAnotherUser()
    {
        using var filler = Context.CreateExecutor();
        var anotherUserId = await filler.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var request = new UpdateRecordPeriodRequest(_record.Id, _record.Periods[0].Id)
        {
            Description = new UpdateField<RecordPeriodDescription?>(RecordPeriodDescription.Parse("foreign", null))
        };

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(anotherContext, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
        Assert.Null((await GetRecordAsync()).Periods[0].Description);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodOverlapsAnother()
    {
        var secondId = await AddPeriodAsync(CreateAfterRecord(2, 1));
        var first = _record.Periods[0].Period;
        var period = new DateTimePeriod(first.EndAt.AddMinutes(-1), first.EndAt.AddHours(3));
        var request = new UpdateRecordPeriodRequest(_record.Id, secondId)
        {
            Period = new UpdateField<DateTimePeriod>(period)
        };

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodHasZeroDuration()
    {
        var current = _record.Periods[0];
        var request = new UpdateRecordPeriodRequest(_record.Id, current.Id)
        {
            Period = new UpdateField<DateTimePeriod>(new DateTimePeriod(current.Period.StartAt, current.Period.StartAt))
        };

        var response = await Context.ExecuteAsync<UpdateRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }
}

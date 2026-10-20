using Freem.Domain;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Contracts.Records.RemovePeriod;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;
using Record = Freem.Domain.Entities.Records.Record;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RecordsRemovePeriodUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private Record _record = null!;

    public RecordsRemovePeriodUseCaseTests(TestContext context)
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
        var secondId = await AddPeriodAsync(CreateAfterRecord(1, 1));
        var request = new RemoveRecordPeriodRequest(_record.Id, secondId);

        var response = await Context.ExecuteAsync<RemoveRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.True(response.Success);

        var actual = await GetRecordAsync();
        var actualPeriod = Assert.Single(actual.Periods);
        Assert.Equal(_record.Periods[0].Id, actualPeriod.Id);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordBelongsToAnotherUser()
    {
        var secondId = await AddPeriodAsync(CreateAfterRecord(1, 1));
        using var filler = Context.CreateExecutor();
        var anotherUserId = await filler.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var request = new RemoveRecordPeriodRequest(_record.Id, secondId);

        var response = await Context.ExecuteAsync<RemoveRecordPeriodRequest, Result<Error>>(anotherContext, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
        Assert.Equal(2, (await GetRecordAsync()).Periods.Count);
    }

    [Fact]
    public async Task ShouldFailure_WhenLastPeriod()
    {
        var request = new RemoveRecordPeriodRequest(_record.Id, _record.Periods[0].Id);

        var response = await Context.ExecuteAsync<RemoveRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
        Assert.Single((await GetRecordAsync()).Periods);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodDoesNotExist()
    {
        var request = new RemoveRecordPeriodRequest(_record.Id, RecordPeriodId.Generate());

        var response = await Context.ExecuteAsync<RemoveRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var request = new RemoveRecordPeriodRequest(RecordId.Generate(), _record.Periods[0].Id);

        var response = await Context.ExecuteAsync<RemoveRecordPeriodRequest, Result<Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
}

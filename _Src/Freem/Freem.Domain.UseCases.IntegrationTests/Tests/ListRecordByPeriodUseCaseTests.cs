using Freem.Domain.Entities.Records.Comparers;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.ListByPeriod;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ListRecordByPeriodUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;
    
    private HandlerContext _context = null!;
    private IReadOnlyList<Domain.Entities.Records.Record> _records = null!;
    
    public ListRecordByPeriodUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        var activity = await filler.Activities.CreateAsync(_context);
        var records = await filler.Records.CreateManyAsync(_context, [activity.Id], RecordsCount);
        _records = records
            .OrderBy(e => e.Periods.Min(p => p.Period.StartAt))
            .ThenBy(e => e.Periods.Max(p => p.Period.EndAt))
            .ThenBy(e => e.Id.ToString())
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var startAt = Context.GetCurrentDateTime().AddDays(-10);
        var period = new DateTimePeriod(startAt, startAt.AddDays(10));

        var request = new ListRecordByPeriodRequest(period);

        var response = await Context.ExecuteAsync<ListRecordByPeriodRequest, Result<ListRecordByPeriodResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Records);
        Assert.NotNull(response.Value.TotalCount);
        Assert.Null(response.Error);
        
        Assert.Equal(RecordsCount, (int)response.Value.TotalCount);
        Assert.Equal(RecordsCount, response.Value.Records.Count);
        Assert.Equal(_records, response.Value.Records, RecordEqualityComparer.Instance);
    }
    
}
using Freem.Domain.Entities.Records.Comparers;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.PeriodList;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class PeriodListRecordUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;
    
    private HandlerContext _context = null!;
    private IReadOnlyList<Domain.Entities.Records.Record> _records = null!;
    
    public PeriodListRecordUseCaseTests(TestContext context) 
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
            .OrderBy(e => e.Period.StartAt)
            .ThenBy(e => e.Period.EndAt)
            .ThenBy(e => e.Id.ToString())
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var startAt = Context.GetCurrentDateTime().AddDays(-10);
        var period = new DateTimePeriod(startAt, startAt.AddDays(10));

        var request = new PeriodListRequest(period);

        var response = await Context.ExecuteAsync<PeriodListRequest, Result<PeriodListResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Records);
        Assert.NotNull(response.Value.TotalCount);
        Assert.Null(response.Error);
        
        Assert.Equal(RecordsCount, (int)response.Value.TotalCount);
        Assert.Equal(RecordsCount, response.Value.Records.Count);
        Assert.Equal(_records, response.Value.Records, new RecordEqualityComparer());
    }
    
}
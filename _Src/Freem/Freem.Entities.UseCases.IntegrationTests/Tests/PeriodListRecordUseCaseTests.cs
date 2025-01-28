using Freem.Entities.Records.Comparers;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class PeriodListRecordUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;
    
    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Entities.Records.Record> _records;
    
    public PeriodListRecordUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();

        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);

        var activity = filler.Activities.Create(_context);
        var records = filler.Records.CreateMany(_context, [activity.Id], RecordsCount);
        _records = records
            .OrderBy(e => e.Period.StartAt)
            .ThenBy(e => e.Period.EndAt)
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var startAt = DateTimeOffset.UtcNow.AddDays(-10);
        var period = new DateTimePeriod(startAt, startAt.AddDays(10));

        var request = new PeriodListRequest(period);

        var response = await Context.ExecuteAsync<PeriodListRequest, PeriodListResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Records);
        Assert.NotNull(response.TotalCount);
        Assert.Null(response.Error);
        
        Assert.Equal(RecordsCount, (int)response.TotalCount);
        Assert.Equal(RecordsCount, response.Records.Count);
        Assert.Equal(_records, response.Records, new RecordEqualityComparer());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var period = new DateTimePeriod(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
        var request = new PeriodListRequest(period);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<PeriodListRequest, PeriodListResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
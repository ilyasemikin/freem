using Freem.Domain.Entities.Records.Comparers;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsListUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;
    
    private HandlerContext _context = null!;
    private IReadOnlyList<Domain.Entities.Records.Record> _records = null!;
    
    public RecordsListUseCaseTests(TestContext context) 
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
            .OrderBy(e => e.Id.ToString())
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListRecordRequest();
        
        var response = await Context.ExecuteAsync<ListRecordRequest, Result<ListRecordResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Records);
        Assert.NotNull(response.Value.TotalCount);
        Assert.Null(response.Error);

        var orderedRecords = response.Value.Records
            .OrderBy(record => record.Id.ToString())
            .ToArray();
        
        Assert.Equal(RecordsCount, (int)response.Value.TotalCount);
        Assert.Equal(RecordsCount, orderedRecords.Length);
        Assert.Equal(_records, orderedRecords, new RecordEqualityComparer());
    }
    
}
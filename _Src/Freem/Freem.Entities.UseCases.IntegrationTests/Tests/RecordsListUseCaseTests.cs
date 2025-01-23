using Freem.Entities.Records.Comparers;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsListUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;
    
    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Entities.Records.Record> _records;
    
    public RecordsListUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        var records = filler.Records.CreateMany(_context, [activity.Id], RecordsCount);
        _records = records
            .OrderBy(e => (string)e.Id)
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListRecordRequest();
        
        var response = await Context.ExecuteAsync<ListRecordRequest, ListRecordResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Records);
        Assert.NotNull(response.TotalCount);
        Assert.Null(response.Error);

        var orderedRecords = response.Records
            .OrderBy(record => (string)record.Id)
            .ToArray();
        
        Assert.Equal(RecordsCount, orderedRecords.Length);
        Assert.Equal(_records, orderedRecords, new RecordEqualityComparer());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new ListRecordRequest();
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<ListRecordRequest, ListRecordResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
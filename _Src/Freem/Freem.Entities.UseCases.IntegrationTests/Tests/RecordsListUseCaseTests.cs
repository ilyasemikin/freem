using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Comparers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Entities.UseCases.Records.List.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsListUseCaseTests : UseCaseTestBase
{
    private const int RecordsCount = 10;
    
    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Entities.Records.Record> _records;
    
    public RecordsListUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        var records = services.Samples.Records
            .CreateMany(userId, activity.Id, RecordsCount)
            .ToArray();

        _context = new UseCaseExecutionContext(userId);
        _records = records
            .OrderBy(e => (string)e.Id)
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListRecordRequest();
        
        var response = await Services.RequestExecutor.ExecuteAsync<ListRecordRequest, ListRecordResponse>(_context, request);

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
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<ListRecordRequest, ListRecordResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
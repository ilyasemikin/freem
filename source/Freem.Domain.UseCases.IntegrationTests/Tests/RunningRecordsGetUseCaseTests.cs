using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Entities.RunningRecords.Comparers;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Get;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsGetUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private RunningRecord _record = null!;
    
    public RunningRecordsGetUseCaseTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        var record = await filler.RunningRecords.StartAsync(_context, [activity.Id]);
        _record = record;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetRunningRecordRequest();

        var response = await Context.ExecuteAsync<GetRunningRecordRequest, Result<GetRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Record);
        Assert.Null(response.Error);

        Assert.Equal(_record, response.Value.Record, RunningRecordEqualityComparer.Instance);
    }
    
}
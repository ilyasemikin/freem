using Freem.Domain;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStopUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;

    private RunningRecord _record = null!;
    
    public RunningRecordsStopUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        _record = await filler.RunningRecords.StartAsync(_context, [activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);

        var response = await Context.ExecuteAsync<StopRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var result = await executor.RunningRecords.GetAsync(_context);
        
        Assert.False(result.Success);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingRunning()
    {
        using var executor = Context.CreateExecutor();
        await executor.RunningRecords.RemoveAsync(_context);
        
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Context.ExecuteAsync<StopRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenEndAtTooEarly()
    {
        var endAt = _record.StartAt.AddDays(-1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Context.ExecuteAsync<StopRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }
    
}
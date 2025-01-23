using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStopUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;

    private readonly RunningRecord _record;
    
    public RunningRecordsStopUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        _record = filler.RunningRecords.Start(_context, [activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);

        var response = await Context.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var result = executor.RunningRecords.Get(_context);
        
        Assert.False(result.Success);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingRunning()
    {
        using var executor = Context.CreateExecutor();
        executor.RunningRecords.Remove(_context);
        
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Context.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StopRunningRecordErrorCode.NothingToStop, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenEndAtTooEarly()
    {
        var endAt = _record.StartAt.AddDays(-1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Context.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StopRunningRecordErrorCode.EndAtToEarly, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
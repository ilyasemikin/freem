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
    private readonly UserIdentifier _userId;

    private readonly RunningRecord _record;
    
    public RunningRecordsStopUseCaseTests(ServicesContext services) 
        : base(services)
    {
        using var filler = Services.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        _record = filler.RunningRecords.Start(_context, [activity.Id]);
        
        _userId = userId;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);

        var response = await Services.RequestExecutor.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var existed = Services.Samples.RunningRecords.TryGet(_userId, out _);
        
        Assert.False(existed);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingRunning()
    {
        Services.Samples.RunningRecords.Remove(_userId);
        
        var endAt = _record.StartAt.AddDays(1);
        var request = new StopRunningRecordRequest(endAt);
        
        var response = await Services.RequestExecutor.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
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
        
        var response = await Services.RequestExecutor.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
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

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
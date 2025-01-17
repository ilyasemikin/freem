using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.RunningRecords.Stop;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStopUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;

    private readonly DateTimeOffset _endAt;
    
    public RunningRecordsStopUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        services.Samples.RunningRecords.Start(userId, activity.Id);

        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
        
        _endAt = DateTimeOffset.UtcNow;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new StopRunningRecordRequest(_endAt);

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
        
        var request = new StopRunningRecordRequest(_endAt);
        
        var response = await Services.RequestExecutor.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StopRunningRecordErrorCode.NothingToStop, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenEndAtTooEarly()
    {
        var endAt = _endAt.AddDays(-1);
        
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
        var request = new StopRunningRecordRequest(_endAt);

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.RunningRecords.Remove.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsRemoveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    
    public RunningRecordsRemoveUseCase(ServicesContext services) : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        services.Samples.RunningRecords.Start(userId, activity.Id);

        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveRunningRecordRequest();
        
        var response = await Services.RequestExecutor.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        Services.Samples.RunningRecords.Remove(_userId);
        
        var request = new RemoveRunningRecordRequest();
        
        var response = await Services.RequestExecutor.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(RemoveRunningRecordErrorCode.RunningRecordNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveRunningRecordRequest();

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
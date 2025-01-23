using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsRemoveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    
    public RunningRecordsRemoveUseCase(ServicesContext services) : base(services)
    {
        using var filler = Services.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        filler.RunningRecords.Start(_context, [activity.Id]);
        
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
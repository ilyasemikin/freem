using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsRemoveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    
    public RunningRecordsRemoveUseCase(TestContext context) : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        filler.RunningRecords.Start(_context, [activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveRunningRecordRequest();
        
        var response = await Context.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        using var executor = Context.CreateExecutor();
        executor.RunningRecords.Remove(_context);
        
        var request = new RemoveRunningRecordRequest();
        
        var response = await Context.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(RemoveRunningRecordErrorCode.RunningRecordNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveRunningRecordRequest();

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsRemoveUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly RecordIdentifier _recordId;
    
    public RecordsRemoveUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        var record = filler.Records.Create(_context, [activity.Id]);
        _recordId = record.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveRecordRequest(_recordId);
        
        var response = await Context.ExecuteAsync<RemoveRecordRequest, RemoveRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var notExistedRecordId = Context.CreateIdentifier<RecordIdentifier>();
            
        var request = new RemoveRecordRequest(notExistedRecordId);
        
        var response = await Context.ExecuteAsync<RemoveRecordRequest, RemoveRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RemoveRecordErrorCode.RecordNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveRecordRequest(_recordId);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<RemoveRecordRequest, RemoveRecordResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
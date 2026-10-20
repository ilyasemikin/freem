using Freem.Domain;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsRemoveUseCase : UseCaseTestBase
{
    private HandlerContext _context = null!;
    
    public RunningRecordsRemoveUseCase(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        await filler.RunningRecords.StartAsync(_context, [activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveRunningRecordRequest();
        
        var response = await Context.ExecuteAsync<RemoveRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        using var executor = Context.CreateExecutor();
        await executor.RunningRecords.RemoveAsync(_context);
        
        var request = new RemoveRunningRecordRequest();
        
        var response = await Context.ExecuteAsync<RemoveRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
    
}
using Freem.Domain;
using Freem.Domain.Records;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsRemoveUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private RecordId _recordId = null!;
    
    public RecordsRemoveUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        var record = await filler.Records.CreateAsync(_context, [activity.Id]);
        _recordId = record.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveRecordRequest(_recordId);
        
        var response = await Context.ExecuteAsync<RemoveRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var notExistedRecordId = RecordId.Generate();
            
        var request = new RemoveRecordRequest(notExistedRecordId);
        
        var response = await Context.ExecuteAsync<RemoveRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
    
}
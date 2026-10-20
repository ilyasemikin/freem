using Freem.Domain;
using Freem.Domain.Entities.Records.Comparers;
using Freem.Domain.Records;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsGetUseCase : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private Domain.Entities.Records.Record _record = null!;
    
    public RecordsGetUseCase(TestContext context) 
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
        _record = record;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetRecordRequest(_record.Id);

        var response = await Context.ExecuteAsync<GetRecordRequest, Result<GetRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Record);
        
        Assert.Equal(_record, response.Value.Record, new RecordEqualityComparer());
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var notExistedRecordId = RecordId.Generate();
        
        var request = new GetRecordRequest(notExistedRecordId);
        
        var response = await Context.ExecuteAsync<GetRecordRequest, Result<GetRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
    
}
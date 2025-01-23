using Freem.Entities.Records.Comparers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsGetUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly Entities.Records.Record _record;
    
    public RecordsGetUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        var record = filler.Records.Create(_context, [activity.Id]);
        _record = record;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetRecordRequest(_record.Id);

        var response = await Context.ExecuteAsync<GetRecordRequest, GetRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Record);
        
        Assert.Equal(_record, response.Record, new RecordEqualityComparer());
    }

    [Fact]
    public async Task ShouldFailure_WhenRecordDoesNotExist()
    {
        var notExistedRecordId = Context.CreateIdentifier<RecordIdentifier>();
        
        var request = new GetRecordRequest(notExistedRecordId);
        
        var response = await Context.ExecuteAsync<GetRecordRequest, GetRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);
        
        Assert.Equal(GetRecordErrorCode.RecordNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new GetRecordRequest(_record.Id);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<GetRecordRequest, GetRecordResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
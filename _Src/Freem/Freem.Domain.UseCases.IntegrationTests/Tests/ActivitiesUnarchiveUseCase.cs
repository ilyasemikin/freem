using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Unarchive;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUnarchiveUseCase : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private ActivityId _activityId = null!;
    
    public ActivitiesUnarchiveUseCase(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        _activityId = activity.Id;

        await filler.Activities.ArchiveAsync(_context, activity.Id);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new UnarchiveActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<UnarchiveActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = await executor.Activities.RequiredGetAsync(_context, _activityId);
        
        Assert.Equal(ActivityStatus.Active, actual.Status);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var request = new UnarchiveActivityRequest(notExistedActivityId);

        var response = await Context.ExecuteAsync<UnarchiveActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenUnarchiveAlreadyActive()
    {
        using var executor = Context.CreateExecutor();
        await executor.Activities.UnarchiveAsync(_context, _activityId);
        
        var request = new UnarchiveActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<UnarchiveActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.ActivityInvalidStatus, response.Error.Type);
    }
    
}
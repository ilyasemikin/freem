using Freem.Entities.Identifiers;
using Freem.Entities.Models.Activities;
using Freem.Entities.UseCases.Contracts.Activities.Unarchive;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUnarchiveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;
    
    public ActivitiesUnarchiveUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        _activityId = activity.Id;

        filler.Activities.Archive(_context, activity.Id);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new UnarchiveActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Activities.RequiredGet(_context, _activityId);
        
        Assert.Equal(ActivityStatus.Active, actual.Status);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var request = new UnarchiveActivityRequest(notExistedActivityId);

        var response = await Context.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal(UnarchiveActivityErrorCode.ActivityNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenUnarchiveAlreadyActive()
    {
        using var executor = Context.CreateExecutor();
        executor.Activities.Unarchive(_context, _activityId);
        
        var request = new UnarchiveActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UnarchiveActivityErrorCode.ActivityInvalidStatus, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UnarchiveActivityRequest(_activityId);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
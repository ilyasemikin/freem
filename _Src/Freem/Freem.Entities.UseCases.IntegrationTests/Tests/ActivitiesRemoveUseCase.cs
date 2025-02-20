using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesRemoveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;
    
    public ActivitiesRemoveUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        _activityId = activity.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveActivityRequest(_activityId);

        var response = await Context.ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var request = new RemoveActivityRequest(notExistedActivityId);
        
        var response = await Context.ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RemoveActivityErrorCode.ActivityNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveActivityRequest(_activityId);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
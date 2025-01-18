using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesRemoveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    private readonly ActivityIdentifier _activityId;
    
    public ActivitiesRemoveUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
        _activityId = activity.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveActivityRequest(_activityId);

        var response = await Services.RequestExecutor.ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var request = new RemoveActivityRequest(notExistedActivityId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RemoveActivityErrorCode.ActivityNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveActivityRequest(_activityId);
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
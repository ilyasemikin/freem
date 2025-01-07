using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.Activities.Archive.Models;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.Activities.Unarchive.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUnarchiveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    private readonly ActivityIdentifier _activityId;
    
    public ActivitiesUnarchiveUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        services.Samples.Activities.Archive(userId, activity.Id);

        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
        _activityId = activity.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new UnarchiveActivityRequest(_activityId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Activities.Get(_userId, _activityId);
        
        Assert.Equal(ActivityStatus.Active, actual.Status);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var request = new UnarchiveActivityRequest(notExistedActivityId);

        var response = await Services.RequestExecutor.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal(UnarchiveActivityErrorCode.ActivityNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenUnarchiveAlreadyActive()
    {
        Services.Samples.Activities.Unarchive(_userId, _activityId);
        
        var request = new UnarchiveActivityRequest(_activityId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UnarchiveActivityErrorCode.ActivityInvalidStatus, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UnarchiveActivityRequest(_activityId);
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
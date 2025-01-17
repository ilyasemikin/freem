using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Activities.Archive;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesArchiveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    private readonly ActivityIdentifier _activityId;

    public ActivitiesArchiveUseCase(ServicesContext services)
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
        var request = new ArchiveActivityRequest(_activityId);

        var response = await Services.RequestExecutor.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Activities.Get(_userId, _activityId);

        Assert.Equal(ActivityStatus.Archived, actual.Status);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var request = new ArchiveActivityRequest(notExistedActivityId);

        var response = await Services.RequestExecutor.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal(ArchiveActivityErrorCode.ActivityNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenArchiveAlreadyArchived()
    {
        Services.Samples.Activities.Archive(_userId, _activityId);
        
        var request = new ArchiveActivityRequest(_activityId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(ArchiveActivityErrorCode.ActivityInvalidStatus, response.Error.Code);
    }

    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new ArchiveActivityRequest(_activityId);

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
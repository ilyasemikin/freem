using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesArchiveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;

    public ActivitiesArchiveUseCase(TestContext context)
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
        var request = new ArchiveActivityRequest(_activityId);

        var response = await Context.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Activities.RequiredGet(_context, _activityId);

        Assert.Equal(ActivityStatus.Archived, actual.Status);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var request = new ArchiveActivityRequest(notExistedActivityId);

        var response = await Context.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal(ArchiveActivityErrorCode.ActivityNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenArchiveAlreadyArchived()
    {
        using var executor = Context.CreateExecutor();
        executor.Activities.Archive(_context, _activityId);
        
        var request = new ArchiveActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(ArchiveActivityErrorCode.ActivityInvalidStatus, response.Error.Code);
    }

    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new ArchiveActivityRequest(_activityId);

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
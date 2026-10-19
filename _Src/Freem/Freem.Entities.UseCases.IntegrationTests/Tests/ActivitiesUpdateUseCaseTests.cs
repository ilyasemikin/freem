using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly ActivityName NewName = ActivityName.Parse("updated_activity", null);
    
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityId _activityId;
    private readonly TagId _updatedTagId;
    
    public ActivitiesUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.Users.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        _activityId = activity.Id;
        
        var tag = filler.Tags.Create(_context);
        _updatedTagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateActivityRequest(_activityId)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };

        var response = await Context.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Activities.RequiredGet(_context, _activityId);
        
        Assert.Equal(NewName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagIdsCollection([_updatedTagId]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Activities.RequiredGet(_context, _activityId);

        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedIdsCollection<TagId>.Equals);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var request = new UpdateActivityRequest(notExistedActivityId)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateActivityErrorCode.ActivityNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateActivityErrorCode.RelatedTagsNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);

        var tags = new RelatedTagIdsCollection([tag.Id]);
        
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateActivityErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateActivityErrorCode.NothingToUpdate, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateActivityRequest(_activityId);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
﻿using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUpdateUseCaseTests : UseCaseTestBase
{
    private const string NewName = "updated_activity";

    private const string AnotherUserLogin = "another_user";
    
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    private readonly ActivityIdentifier _activityId;
    private readonly TagIdentifier _updatedTagId;
    
    public ActivitiesUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        
        var tag = services.Samples.Tags.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
        _activityId = activity.Id;
        _updatedTagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateActivityRequest(_activityId)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };

        var response = await Services.RequestExecutor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Activities.Get(_userId, _activityId);
        
        Assert.Equal(NewName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagsCollection([_updatedTagId]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Activities.Get(_userId, _activityId);

        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var request = new UpdateActivityRequest(notExistedActivityId)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateActivityErrorCode.ActivityNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var tags = new RelatedTagsCollection([notExistedTagId]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateActivityErrorCode.RelatedTagsNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var tag = Services.Samples.Tags.Create(anotherUserId);
        var tags = new RelatedTagsCollection([tag]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateActivityErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateActivityRequest(_activityId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateActivityErrorCode.NothingToUpdate, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateActivityRequest(_activityId);
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
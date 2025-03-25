using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "record_name";
    private const string UpdatedDescription = "record_description";
    
    private readonly UseCaseExecutionContext _context;
    
    private readonly ActivityIdentifier _updatedActivityId;
    private readonly TagIdentifier _updatedTagId;
    
    public RunningRecordsUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        filler.RunningRecords.Start(_context, [activity.Id]);
        
        var updateActivity = filler.Activities.Create(_context);
        _updatedActivityId = updateActivity.Id;
        
        var updateTag = filler.Tags.Create(_context);
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName?>(UpdatedName)
        };

        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.RunningRecords.RequiredGet(_context);
        
        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRunningRecordRequest
        {
            Description = new UpdateField<RecordDescription?>(UpdatedDescription)
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.RunningRecords.RequiredGet(_context);
        
        Assert.Equal(UpdatedDescription, actual.Description);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateActivities()
    {
        var activities = new RelatedActivitiesCollection([_updatedActivityId]);
        var request = new UpdateRunningRecordRequest
        {
            Activities = new UpdateField<RelatedActivitiesCollection>(activities)
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = executor.RunningRecords.RequiredGet(_context);
        
        Assert.Equal(activities, actual.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagsCollection([_updatedTagId]);
        var request = new UpdateRunningRecordRequest
        {
            Tags = new UpdateField<RelatedTagsCollection>(tags)
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = executor.RunningRecords.RequiredGet(_context);
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        using var executor = Context.CreateExecutor();
        executor.RunningRecords.Remove(_context);
        
        var request = new UpdateRunningRecordRequest
        {
            Name = (RecordName)UpdatedName
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RunningRecordNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);

        var request = new UpdateRunningRecordRequest
        {
            Activities = activities
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivitiesCollection([activity]);

        var request = new UpdateRunningRecordRequest
        {
            Activities = activities
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();
        var tags = new RelatedTagsCollection([notExistedTagId]);

        var request = new UpdateRunningRecordRequest
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagsCollection([tag]);

        var request = new UpdateRunningRecordRequest
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateRunningRecordRequest();
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.NothingToUpdate, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateRunningRecordRequest
        {
            Tags = new RelatedTagsCollection()
        };

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
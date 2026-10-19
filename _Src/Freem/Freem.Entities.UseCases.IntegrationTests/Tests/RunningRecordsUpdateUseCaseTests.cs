using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly RecordName UpdatedName = RecordName.Parse("record_name", null);
    private static readonly RecordDescription UpdatedDescription = RecordDescription.Parse("record_description", null);
    
    private readonly UseCaseExecutionContext _context;
    
    private readonly ActivityId _updatedActivityId;
    private readonly TagId _updatedTagId;
    
    public RunningRecordsUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.Users.Register();
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
        var activities = new RelatedActivityIdsCollection([_updatedActivityId]);
        var request = new UpdateRunningRecordRequest
        {
            Activities = new UpdateField<RelatedActivityIdsCollection>(activities)
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = executor.RunningRecords.RequiredGet(_context);
        
        Assert.Equal(activities, actual.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagIdsCollection([_updatedTagId]);
        var request = new UpdateRunningRecordRequest
        {
            Tags = new UpdateField<RelatedTagIdsCollection>(tags)
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = executor.RunningRecords.RequiredGet(_context);
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedIdsCollection<TagId>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        using var executor = Context.CreateExecutor();
        executor.RunningRecords.Remove(_context);
        
        var request = new UpdateRunningRecordRequest
        {
            Name = UpdatedName
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
        var notExistedActivityId = ActivityId.Generate();
        var activities = new RelatedActivityIdsCollection([notExistedActivityId]);

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
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivityIdsCollection([activity.Id]);

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
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);

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
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagIdsCollection([tag.Id]);

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
            Tags = new RelatedTagIdsCollection()
        };

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
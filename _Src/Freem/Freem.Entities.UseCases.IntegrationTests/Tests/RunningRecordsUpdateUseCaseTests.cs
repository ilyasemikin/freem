using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Abstractions.Models;
using Freem.Entities.UseCases.DTO.RunningRecords.Update;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "record_name";
    private const string UpdatedDescription = "record_description";

    private const string AnotherUserLogin = "another_user";
    
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    
    private readonly ActivityIdentifier _updatedActivityId;
    private readonly TagIdentifier _updatedTagId;
    
    public RunningRecordsUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        services.Samples.RunningRecords.Start(userId, activity.Id);
        
        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
        
        var updateActivity = services.Samples.Activities.Create(userId);
        var updateTag = services.Samples.Tags.Create(userId);
        
        _updatedActivityId = updateActivity.Id;
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName>(UpdatedName)
        };

        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.RunningRecords.Get(_userId);
        
        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRunningRecordRequest
        {
            Description = new UpdateField<RecordDescription>(UpdatedDescription)
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        var actual = Services.Samples.RunningRecords.Get(_userId);
        
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
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        var actual = Services.Samples.RunningRecords.Get(_userId);
        
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
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        var actual = Services.Samples.RunningRecords.Get(_userId);
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        Services.Samples.RunningRecords.Remove(_userId);
        
        var request = new UpdateRunningRecordRequest
        {
            Name = (RecordName)UpdatedName
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RunningRecordNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);

        var request = new UpdateRunningRecordRequest
        {
            Activities = activities
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var activity = Services.Samples.Activities.Create(anotherUserId);
        var activities = new RelatedActivitiesCollection([activity]);

        var request = new UpdateRunningRecordRequest
        {
            Activities = activities
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var tags = new RelatedTagsCollection([notExistedTagId]);

        var request = new UpdateRunningRecordRequest
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var tag = Services.Samples.Tags.Create(anotherUserId);
        var tags = new RelatedTagsCollection([tag]);

        var request = new UpdateRunningRecordRequest
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateRunningRecordRequest();
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(_context, request);
        
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

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
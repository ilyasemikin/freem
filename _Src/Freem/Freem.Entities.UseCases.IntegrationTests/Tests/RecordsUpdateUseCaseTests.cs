using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "record";
    private const string UpdatedDescription = "description";

    private const string AnotherUserLogin = "another_user";

    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    private readonly RecordIdentifier _recordId;
    
    private readonly ActivityIdentifier _updatedActivityId;
    private readonly TagIdentifier _updatedTagId;
    
    public RecordsUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        var record = services.Samples.Records.Create(userId, activity.Id);
        
        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
        _recordId = record.Id;
        
        var updatedActivity = services.Samples.Activities.Create(userId);
        var updateTag = services.Samples.Tags.Create(userId);
        
        _updatedActivityId = updatedActivity.Id;
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Name = new UpdateField<RecordName>(UpdatedName)
        };

        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Records.Get(_userId, _recordId);
        
        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Description = new UpdateField<RecordDescription>(UpdatedDescription)
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Records.Get(_userId, _recordId);
        
        Assert.Equal(UpdatedDescription, actual.Description);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateActivities()
    {
        var activities = new RelatedActivitiesCollection([_updatedActivityId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = new UpdateField<RelatedActivitiesCollection>(activities)
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Records.Get(_userId, _recordId);
        
        Assert.Equal(activities, actual.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagsCollection([_updatedTagId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Records.Get(_userId, _recordId);
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = activities
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var activity = Services.Samples.Activities.Create(anotherUserId);
        var activities = new RelatedActivitiesCollection([activity]);

        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = activities
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var tags = new RelatedTagsCollection([notExistedTagId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var tag = Services.Samples.Tags.Create(anotherUserId);
        var tags = new RelatedTagsCollection([tag]);

        var request = new UpdateRecordRequest(_recordId)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateRecordRequest(_recordId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateRecordErrorCode.NothingToUpdate, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Name = new UpdateField<RecordName>(UpdatedName),
        };
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
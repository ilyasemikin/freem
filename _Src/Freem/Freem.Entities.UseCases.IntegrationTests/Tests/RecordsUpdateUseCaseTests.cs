using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.Models.Records;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "record";
    private const string UpdatedDescription = "description";

    private readonly UseCaseExecutionContext _context;
    private readonly RecordIdentifier _recordId;
    
    private readonly ActivityIdentifier _updatedActivityId;
    private readonly TagIdentifier _updatedTagId;
    
    public RecordsUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        var record = filler.Records.Create(_context, [activity.Id]);
        _recordId = record.Id;
        
        var updatedActivity = filler.Activities.Create(_context);
        _updatedActivityId = updatedActivity.Id;
        
        var updateTag = filler.Tags.Create(_context);
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Name = new UpdateField<RecordName?>(UpdatedName)
        };

        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Records.RequiredGet(_context, _recordId);
        
        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Description = new UpdateField<RecordDescription?>(UpdatedDescription)
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Records.RequiredGet(_context, _recordId);
        
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
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Records.RequiredGet(_context, _recordId);
        
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
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Records.RequiredGet(_context, _recordId);
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = activities
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivitiesCollection([activity]);

        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = activities
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();
        var tags = new RelatedTagsCollection([notExistedTagId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagsCollection([tag]);

        var request = new UpdateRecordRequest(_recordId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateRecordRequest(_recordId);
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
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
            Name = new UpdateField<RecordName?>(UpdatedName),
        };
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
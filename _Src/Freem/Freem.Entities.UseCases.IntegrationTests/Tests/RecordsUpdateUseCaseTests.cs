using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly RecordName UpdatedName = RecordName.Parse("record", null);
    private static readonly RecordDescription UpdatedDescription = RecordDescription.Parse("description", null);

    private readonly UseCaseExecutionContext _context;
    private readonly RecordId _recordId;
    
    private readonly ActivityId _updatedActivityId;
    private readonly TagId _updatedTagId;
    
    public RecordsUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.Users.Register();
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
    public async Task ShouldSuccess_WhenUpdatePeriod()
    {
        var startAt = DateTimeOffset.UnixEpoch.AddYears(55);
        var endAt = startAt.AddHours(2);
        var period = new DateTimePeriod(startAt, endAt);
        var request = new UpdateRecordRequest(_recordId)
        {
            Period = new UpdateField<DateTimePeriod>(period)
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Records.RequiredGet(_context, _recordId);
        
        Assert.Equal(startAt, actual.Period.StartAt);
        Assert.Equal(endAt, actual.Period.EndAt);
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
        var activities = new RelatedActivityIdsCollection([_updatedActivityId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = new UpdateField<RelatedActivityIdsCollection>(activities)
        };
        
        var response = await Context.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Records.RequiredGet(_context, _recordId);
        
        Assert.Equal(activities, actual.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagIdsCollection([_updatedTagId]);
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
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedIdsCollection<TagId>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var activities = new RelatedActivityIdsCollection([notExistedActivityId]);
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
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivityIdsCollection([activity.Id]);

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
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);
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
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagIdsCollection([tag.Id]);

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
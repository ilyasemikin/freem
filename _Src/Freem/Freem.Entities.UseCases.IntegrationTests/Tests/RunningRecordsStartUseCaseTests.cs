using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStartUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;

    private readonly DateTimeOffset _startAt;
    private readonly RelatedActivityIdsCollection _activities;
    
    public RunningRecordsStartUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.Users.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);

        _startAt = Context.GetCurrentDateTime();
        _activities = new RelatedActivityIdsCollection([activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.RunningRecord);
        Assert.Null(response.Error);
        
        Assert.Equal(_startAt, response.RunningRecord.StartAt);
        Assert.Equal(_activities, response.RunningRecord.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAnotherRunningRecordExist()
    {
        using var executor = Context.CreateExecutor();
        executor.RunningRecords.Start(_context, [_activities.Ids.First()]);
        
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.RunningRecord);
        Assert.Null(response.Error);
        
        Assert.Equal(_startAt, response.RunningRecord.StartAt);
        Assert.Equal(_activities, response.RunningRecord.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var activities = new RelatedActivityIdsCollection([notExistedActivityId]);
        
        var request = new StartRunningRecordRequest(_startAt, activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);

        Assert.Equal(StartRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivityIdsCollection([activity.Id]);
        
        var request = new StartRunningRecordRequest(_startAt, activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StartRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);

        var request = new StartRunningRecordRequest(_startAt, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StartRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagIdsCollection([tag.Id]);

        var request = new StartRunningRecordRequest(_startAt, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StartRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new StartRunningRecordRequest(_startAt, _activities);

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
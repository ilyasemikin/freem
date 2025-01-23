using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;
using Freem.Time;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStartUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;

    private readonly DateTimeOffset _startAt;
    private readonly RelatedActivitiesCollection _activities;
    
    public RunningRecordsStartUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);

        _startAt = DateTimeOffset.UtcNow;
        _activities = new RelatedActivitiesCollection([activity.Id]);
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
        
        Assert.True(DateTimeOperations.EqualsUpToSeconds(_startAt, response.RunningRecord.StartAt));
        Assert.Equal(_activities, response.RunningRecord.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAnotherRunningRecordExist()
    {
        using var executor = Context.CreateExecutor();
        executor.RunningRecords.Start(_context, [_activities.Identifiers.First()]);
        
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.RunningRecord);
        Assert.Null(response.Error);
        
        Assert.True(DateTimeOperations.EqualsUpToSeconds(_startAt, response.RunningRecord.StartAt));
        Assert.Equal(_activities, response.RunningRecord.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);
        
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
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivitiesCollection([activity]);
        
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
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();
        var tags = new RelatedTagsCollection([notExistedTagId]);

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
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagsCollection([tag]);

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
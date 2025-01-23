using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;
using Freem.Time;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStartUseCaseTests : UseCaseTestBase
{
    private const string AnotherUserLogin = "another_user";
    
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;

    private readonly DateTimeOffset _startAt;
    private readonly RelatedActivitiesCollection _activities;
    
    public RunningRecordsStartUseCaseTests(ServicesContext services) 
        : base(services)
    {
        using var filler = Services.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);

        _userId = userId;

        _startAt = DateTimeOffset.UtcNow;
        _activities = new RelatedActivitiesCollection([activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
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
        var otherStartAt = _startAt.AddHours(-1);
        Services.Samples.RunningRecords.Start(_userId, _activities.Identifiers.First(), otherStartAt);
        
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
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
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);
        
        var request = new StartRunningRecordRequest(_startAt, activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);

        Assert.Equal(StartRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var activity = Services.Samples.Activities.Create(anotherUserId);
        var activities = new RelatedActivitiesCollection([activity]);
        
        var request = new StartRunningRecordRequest(_startAt, activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StartRunningRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var tags = new RelatedTagsCollection([notExistedTagId]);

        var request = new StartRunningRecordRequest(_startAt, _activities)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.RunningRecord);
        Assert.NotNull(response.Error);
        
        Assert.Equal(StartRunningRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var tag = Services.Samples.Tags.Create(anotherUserId);
        var tags = new RelatedTagsCollection([tag]);

        var request = new StartRunningRecordRequest(_startAt, _activities)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
        
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

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Start;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStartUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;

    private DateTimeOffset _startAt;
    private RelatedActivityIdsCollection _activities = null!;
    
    public RunningRecordsStartUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);

        _startAt = Context.GetCurrentDateTime();
        _activities = new RelatedActivityIdsCollection([activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.RunningRecord);
        Assert.Null(response.Error);
        
        Assert.Equal(_startAt, response.Value.RunningRecord.StartAt);
        Assert.Equal(_activities, response.Value.RunningRecord.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAnotherRunningRecordExist()
    {
        using var executor = Context.CreateExecutor();
        await executor.RunningRecords.StartAsync(_context, [_activities.Ids.First()]);
        
        var request = new StartRunningRecordRequest(_startAt, _activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.RunningRecord);
        Assert.Null(response.Error);
        
        Assert.Equal(_startAt, response.Value.RunningRecord.StartAt);
        Assert.Equal(_activities, response.Value.RunningRecord.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenAnotherRunningRecordStartedLater()
    {
        using var executor = Context.CreateExecutor();
        var current = await executor.RunningRecords.StartAsync(_context, [_activities.Ids.First()]);
        
        var startAt = current.StartAt.AddDays(-1);
        var request = new StartRunningRecordRequest(startAt, _activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);

        var actual = await executor.RunningRecords.RequiredGetAsync(_context);
        Assert.Equal(current.StartAt, actual.StartAt);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var activities = new RelatedActivityIdsCollection([notExistedActivityId]);
        
        var request = new StartRunningRecordRequest(_startAt, activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = await executor.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var activity = await executor.Activities.CreateAsync(anotherContext);
        
        var activities = new RelatedActivityIdsCollection([activity.Id]);
        
        var request = new StartRunningRecordRequest(_startAt, activities);
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
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
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = await executor.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var tag = await executor.Tags.CreateAsync(anotherContext);
        
        var tags = new RelatedTagIdsCollection([tag.Id]);

        var request = new StartRunningRecordRequest(_startAt, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }
    
}
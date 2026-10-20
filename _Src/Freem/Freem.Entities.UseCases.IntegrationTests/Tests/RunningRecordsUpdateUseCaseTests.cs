using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly RecordName UpdatedName = RecordName.Parse("record_name", null);
    private static readonly RecordDescription UpdatedDescription = RecordDescription.Parse("record_description", null);
    
    private HandlerContext _context = null!;
    
    private ActivityId _updatedActivityId = null!;
    private TagId _updatedTagId = null!;
    
    public RunningRecordsUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        await filler.RunningRecords.StartAsync(_context, [activity.Id]);
        
        var updateActivity = await filler.Activities.CreateAsync(_context);
        _updatedActivityId = updateActivity.Id;
        
        var updateTag = await filler.Tags.CreateAsync(_context);
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName?>(UpdatedName)
        };

        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = await executor.RunningRecords.RequiredGetAsync(_context);
        
        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRunningRecordRequest
        {
            Description = new UpdateField<RecordDescription?>(UpdatedDescription)
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = await executor.RunningRecords.RequiredGetAsync(_context);
        
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
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = await executor.RunningRecords.RequiredGetAsync(_context);
        
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
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = await executor.RunningRecords.RequiredGetAsync(_context);
        
        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedIdsCollection<TagId>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenRunningRecordDoesNotExist()
    {
        using var executor = Context.CreateExecutor();
        await executor.RunningRecords.RemoveAsync(_context);
        
        var request = new UpdateRunningRecordRequest
        {
            Name = UpdatedName
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
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
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
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

        var request = new UpdateRunningRecordRequest
        {
            Activities = activities
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
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
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
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

        var request = new UpdateRunningRecordRequest
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateRunningRecordRequest();
        
        var response = await Context.ExecuteAsync<UpdateRunningRecordRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.NothingToDo, response.Error.Type);
    }
    
}
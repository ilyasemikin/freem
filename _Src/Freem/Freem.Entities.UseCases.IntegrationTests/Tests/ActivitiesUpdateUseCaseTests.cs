using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly ActivityName NewName = ActivityName.Parse("updated_activity", null);
    
    private HandlerContext _context = null!;
    private ActivityId _activityId = null!;
    private TagId _updatedTagId = null!;
    
    public ActivitiesUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        _activityId = activity.Id;
        
        var tag = await filler.Tags.CreateAsync(_context);
        _updatedTagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateActivityRequest(_activityId)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };

        var response = await Context.ExecuteAsync<UpdateActivityRequest, Result<Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = await executor.Activities.RequiredGetAsync(_context, _activityId);
        
        Assert.Equal(NewName, actual.Name);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagIdsCollection([_updatedTagId]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = await executor.Activities.RequiredGetAsync(_context, _activityId);

        Assert.Equal(tags, actual.Tags, IReadOnlyRelatedIdsCollection<TagId>.Equals);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenActivityDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var request = new UpdateActivityRequest(notExistedActivityId)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, Result<Error>>(_context, request);
        
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
        
        var request = new UpdateActivityRequest(_activityId)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateActivityRequest(_activityId);
        
        var response = await Context.ExecuteAsync<UpdateActivityRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.NothingToDo, response.Error.Type);
    }
    
}
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Create;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;
using ActivityStatus = Freem.Domain.Activities.Models.ActivityStatus;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesCreateUseCase : UseCaseTestBase
{
    private static readonly ActivityName ActivityName = ActivityName.Parse("activity", null);
    private static readonly ActivityColor ActivityColor = ActivityColor.Parse("#6366f1", null);
    
    private HandlerContext _context = null!;
    
    public ActivitiesCreateUseCase(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateActivityRequest(ActivityName, ActivityColor);
        
        var response = await Context.ExecuteAsync<CreateActivityRequest, Result<CreateActivityResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Activity);
        Assert.Null(response.Error);
        
        Assert.Equal(_context.UserId, response.Value.Activity.UserId);
        Assert.Equal(ActivityName, response.Value.Activity.Name);
        Assert.Equal(ActivityStatus.Active, response.Value.Activity.Status);
        Assert.Equal(0, response.Value.Activity.Tags.Count);
    }

    [Fact]
    public async Task ShouldFailure_WhenRelatedTagsDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);

        var request = new CreateActivityRequest(ActivityName, ActivityColor)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<CreateActivityRequest, Result<CreateActivityResponse, Error>>(_context, request);
        
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

        var request = new CreateActivityRequest(ActivityName, ActivityColor)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<CreateActivityRequest, Result<CreateActivityResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }
}
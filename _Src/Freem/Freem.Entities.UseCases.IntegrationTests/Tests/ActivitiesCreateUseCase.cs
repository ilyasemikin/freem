using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using ActivityStatus = Freem.Entities.Activities.Models.ActivityStatus;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesCreateUseCase : UseCaseTestBase
{
    private const string ActivityName = "activity";

    private const string AnotherUserLogin = "another_user";
    
    private readonly UseCaseExecutionContext _context;
    
    public ActivitiesCreateUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateActivityRequest(ActivityName);
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateActivityRequest, CreateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Activity);
        Assert.Null(response.Error);
        
        Assert.Equal(_context.UserId, response.Activity.UserId);
        Assert.Equal(ActivityName, response.Activity.Name);
        Assert.Equal(ActivityStatus.Active, response.Activity.Status);
        Assert.Equal(0, response.Activity.Tags.Count);
    }

    [Fact]
    public async Task ShouldFailure_WhenRelatedTagsDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var tags = new RelatedTagsCollection([notExistedTagId]);

        var request = new CreateActivityRequest(ActivityName)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateActivityRequest, CreateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Activity);
        Assert.NotNull(response.Error);

        Assert.Equal(CreateActivityErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongsToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var tag = Services.Samples.Tags.Create(anotherUserId);
        var tags = new RelatedTagsCollection([tag]);

        var request = new CreateActivityRequest(ActivityName)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateActivityRequest, CreateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Activity);
        Assert.NotNull(response.Error);
        
        Assert.Equal(CreateActivityErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new CreateActivityRequest(ActivityName);
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<CreateActivityRequest, CreateActivityResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}
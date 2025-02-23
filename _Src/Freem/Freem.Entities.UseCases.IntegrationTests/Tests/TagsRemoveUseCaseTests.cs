using Freem.Entities.Identifiers;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsRemoveUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly TagIdentifier _tagId;
    
    public TagsRemoveUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var tag = filler.Tags.Create(_context);
        _tagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveTagRequest(_tagId);

        var response = await Context.ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();

        var request = new RemoveTagRequest(notExistedTagId);
        
        var response = await Context.ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(RemoveTagErrorCode.TagNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveTagRequest(_tagId);

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
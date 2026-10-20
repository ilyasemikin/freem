using Freem.Domain;
using Freem.Domain.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsRemoveUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private TagId _tagId = null!;
    
    public TagsRemoveUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var tag = await filler.Tags.CreateAsync(_context);
        _tagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveTagRequest(_tagId);

        var response = await Context.ExecuteAsync<RemoveTagRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();

        var request = new RemoveTagRequest(notExistedTagId);
        
        var response = await Context.ExecuteAsync<RemoveTagRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
    
}
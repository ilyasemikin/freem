using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.Tags.Comparers;
using Freem.Domain.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Get;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class TagsGetUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private Tag _tag = null!;
    
    public TagsGetUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var tag = await filler.Tags.CreateAsync(_context);
        _tag = tag;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetTagRequest(_tag.Id);

        var response = await Context.ExecuteAsync<GetTagRequest, Result<GetTagResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Tag);
        Assert.Null(response.Error);
        
        Assert.Equal(_tag, response.Value.Tag, TagEqualityComparer.Instance);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        
        var request = new GetTagRequest(notExistedTagId);
        
        var response = await Context.ExecuteAsync<GetTagRequest, Result<GetTagResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }
    
}
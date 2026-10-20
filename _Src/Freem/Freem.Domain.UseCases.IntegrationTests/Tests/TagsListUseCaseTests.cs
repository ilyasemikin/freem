using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.Tags.Comparers;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.List;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class TagsListUseCaseTests : UseCaseTestBase
{
    private const int TagsCount = 10;

    private HandlerContext _context = null!;
    private IReadOnlyList<Tag> _tags = null!;
    
    public TagsListUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var tags = await filler.Tags.CreateManyAsync(_context, TagsCount);
        _tags = tags
            .OrderBy(x => x.Id.ToString())
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListTagRequest();

        var response = await Context.ExecuteAsync<ListTagRequest, Result<ListTagResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Tags);
        Assert.Null(response.Error);

        var tags = response.Value.Tags
            .OrderBy(x => x.Id.ToString())
            .ToArray();
        
        Assert.Equal(_tags, tags, new TagEqualityComparer());
    }
    
}
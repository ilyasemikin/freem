using Freem.Entities.Tags;
using Freem.Entities.Tags.Comparers;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsListUseCaseTests : UseCaseTestBase
{
    private const int TagsCount = 10;

    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Tag> _tags;
    
    public TagsListUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var tags = filler.Tags.CreateMany(_context, TagsCount);
        _tags = tags
            .OrderBy(x => (string)x.Id)
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListTagRequest();

        var response = await Context.ExecuteAsync<ListTagRequest, ListTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Tags);
        Assert.Null(response.Error);

        var tags = response.Tags
            .OrderBy(x => (string)x.Id)
            .ToArray();
        
        Assert.Equal(_tags, tags, new TagEqualityComparer());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new ListTagRequest();

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<ListTagRequest, ListTagResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
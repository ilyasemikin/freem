using Freem.Entities.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Comparers;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsGetUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly Tag _tag;
    
    public TagsGetUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var tag = filler.Tags.Create(_context);
        _tag = tag;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetTagRequest(_tag.Id);

        var response = await Context.ExecuteAsync<GetTagRequest, GetTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Tag);
        Assert.Null(response.Error);
        
        Assert.Equal(_tag, response.Tag, new TagEqualityComparer());
    }

    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();
        
        var request = new GetTagRequest(notExistedTagId);
        
        var response = await Context.ExecuteAsync<GetTagRequest, GetTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Tag);
        Assert.NotNull(response.Error);
        
        Assert.Equal(GetTagErrorCode.TagNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new GetTagRequest(_tag.Id);

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<GetTagRequest, GetTagResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
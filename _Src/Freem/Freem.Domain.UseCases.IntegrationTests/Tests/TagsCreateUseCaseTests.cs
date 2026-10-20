using Freem.Domain;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class TagsCreateUseCaseTests : UseCaseTestBase
{
    private static readonly TagName TagName = TagName.Parse("tag", null);
    
    private HandlerContext _context = null!;
    
    public TagsCreateUseCaseTests(TestContext context) 
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
        var request = new CreateTagRequest(TagName);

        var response = await Context.ExecuteAsync<CreateTagRequest, Result<CreateTagResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Tag);
        Assert.Null(response.Error);
        
        Assert.Equal(TagName, response.Value.Tag.Name);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagNameAlreadyExists()
    {
        using var executor = Context.CreateExecutor();
        var anotherRequest = new CreateTagRequest(TagName);
        await executor.Tags.CreateAsync(_context, anotherRequest);
        
        var request = new CreateTagRequest(TagName);

        var response = await Context.ExecuteAsync<CreateTagRequest, Result<CreateTagResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.TagNameAlreadyExists, response.Error.Type);
    }
    
}
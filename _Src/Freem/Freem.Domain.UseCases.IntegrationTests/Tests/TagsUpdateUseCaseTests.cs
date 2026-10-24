using Freem.Domain.Entities.Tags;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.Contracts.Tags.Update;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class TagsUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly TagName UpdatedName = TagName.Parse("name", null);
    
    private HandlerContext _context = null!;
    private Tag _tag = null!;
    
    public TagsUpdateUseCaseTests(TestContext context)
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
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = UpdatedName
        };

        var response = await Context.ExecuteAsync<UpdateTagRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = await executor.Tags.RequiredGetAsync(_context, _tag.Id);

        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldFailure_WhenUpdateNameAlreadyExist()
    {
        using var executor = Context.CreateExecutor();
        var anotherRequest = new CreateTagRequest(UpdatedName);
        await executor.Tags.CreateAsync(_context, anotherRequest);
        
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = UpdatedName
        };
        
        var response = await Context.ExecuteAsync<UpdateTagRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.TagNameAlreadyExists, response.Error.Type);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        var request = new UpdateTagRequest(notExistedTagId)
        {
            Name = UpdatedName
        };
        
        var response = await Context.ExecuteAsync<UpdateTagRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateTagRequest(_tag.Id);
        
        var response = await Context.ExecuteAsync<UpdateTagRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.NothingToDo, response.Error.Type);
    }
    
}
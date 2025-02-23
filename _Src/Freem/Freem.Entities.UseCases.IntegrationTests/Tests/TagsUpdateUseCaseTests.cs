using Freem.Entities.Identifiers;
using Freem.Entities.Models.Tags;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "name";
    
    private readonly UseCaseExecutionContext _context;
    private readonly Tag _tag;
    
    public TagsUpdateUseCaseTests(TestContext context) : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var tag = filler.Tags.Create(_context);
        _tag = tag;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = (TagName)UpdatedName
        };

        var response = await Context.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        using var executor = Context.CreateExecutor();
        var actual = executor.Tags.RequiredGet(_context, _tag.Id);

        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldFailure_WhenUpdateNameAlreadyExist()
    {
        using var executor = Context.CreateExecutor();
        var anotherRequest = new CreateTagRequest(UpdatedName);
        executor.Tags.Create(_context, anotherRequest);
        
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = (TagName)UpdatedName
        };
        
        var response = await Context.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateTagErrorCode.TagNameAlreadyExists, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();
        var request = new UpdateTagRequest(notExistedTagId)
        {
            Name = (TagName)UpdatedName
        };
        
        var response = await Context.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateTagErrorCode.TagNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateTagRequest(_tag.Id);
        
        var response = await Context.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateTagErrorCode.NothingToUpdate, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = (TagName)UpdatedName
        };

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
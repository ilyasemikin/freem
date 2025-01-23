using Freem.Entities.Tags;
using Freem.Entities.Tags.Models;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "name";
    
    private readonly UseCaseExecutionContext _context;
    private readonly Tag _tag;
    private readonly UserIdentifier _userId;
    
    public TagsUpdateUseCaseTests(ServicesContext services) : base(services)
    {
        using var filler = Services.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var tag = filler.Tags.Create(_context);
        _tag = tag;
        
        _userId = userId;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = (TagName)UpdatedName
        };

        var response = await Services.RequestExecutor.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);

        var actual = Services.Samples.Tags.Get(_userId, _tag.Id);

        Assert.Equal(UpdatedName, actual.Name);
    }

    [Fact]
    public async Task ShouldFailure_WhenUpdateNameAlreadyExist()
    {
        Services.Samples.Tags.Create(_userId, UpdatedName);
        
        var request = new UpdateTagRequest(_tag.Id)
        {
            Name = (TagName)UpdatedName
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateTagErrorCode.TagNameAlreadyExists, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var request = new UpdateTagRequest(notExistedTagId)
        {
            Name = (TagName)UpdatedName
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateTagErrorCode.TagNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateTagRequest(_tag.Id);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(_context, request);
        
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

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
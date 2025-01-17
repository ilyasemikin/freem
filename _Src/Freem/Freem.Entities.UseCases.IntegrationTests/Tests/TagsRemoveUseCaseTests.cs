using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Tags.Remove;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsRemoveUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly TagIdentifier _tagId;
    
    public TagsRemoveUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var tag = services.Samples.Tags.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _tagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveTagRequest(_tagId);

        var response = await Services.RequestExecutor.ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();

        var request = new RemoveTagRequest(notExistedTagId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(RemoveTagErrorCode.TagNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new RemoveTagRequest(_tagId);

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
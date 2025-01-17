using Freem.Entities.Tags;
using Freem.Entities.Tags.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Tags.Get;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsGetUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly Tag _tag;
    
    public TagsGetUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var tag = services.Samples.Tags.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _tag = tag;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetTagRequest(_tag.Id);

        var response = await Services.RequestExecutor.ExecuteAsync<GetTagRequest, GetTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Tag);
        Assert.Null(response.Error);
        
        Assert.Equal(_tag, response.Tag, new TagEqualityComparer());
    }

    [Fact]
    public async Task ShouldFailure_WhenTagDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        
        var request = new GetTagRequest(notExistedTagId);
        
        var response = await Services.RequestExecutor.ExecuteAsync<GetTagRequest, GetTagResponse>(_context, request);
        
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

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<GetTagRequest, GetTagResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
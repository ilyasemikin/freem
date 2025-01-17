using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Tags.Create;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsCreateUseCaseTests : UseCaseTestBase
{
    private const string TagName = "tag";
    
    private readonly UseCaseExecutionContext _context;
    private readonly UserIdentifier _userId;
    
    public TagsCreateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();

        _context = new UseCaseExecutionContext(userId);
        _userId = userId;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateTagRequest(TagName);

        var response = await Services.RequestExecutor.ExecuteAsync<CreateTagRequest, CreateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Tag);
        Assert.Null(response.Error);
        
        Assert.Equal(TagName, response.Tag.Name);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagNameAlreadyExists()
    {
        Services.Samples.Tags.Create(_userId, TagName);
        
        var request = new CreateTagRequest(TagName);

        var response = await Services.RequestExecutor.ExecuteAsync<CreateTagRequest, CreateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Tag);
        Assert.NotNull(response.Error);

        Assert.Equal(CreateTagErrorCode.TagNameAlreadyExists, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new CreateTagRequest(TagName);

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<CreateTagRequest, CreateTagResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
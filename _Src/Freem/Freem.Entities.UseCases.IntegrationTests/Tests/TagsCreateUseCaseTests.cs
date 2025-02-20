using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsCreateUseCaseTests : UseCaseTestBase
{
    private const string TagName = "tag";
    
    private readonly UseCaseExecutionContext _context;
    
    public TagsCreateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateTagRequest(TagName);

        var response = await Context.ExecuteAsync<CreateTagRequest, CreateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Tag);
        Assert.Null(response.Error);
        
        Assert.Equal(TagName, response.Tag.Name);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagNameAlreadyExists()
    {
        using var executor = Context.CreateExecutor();
        var anotherRequest = new CreateTagRequest(TagName);
        executor.Tags.Create(_context, anotherRequest);
        
        var request = new CreateTagRequest(TagName);

        var response = await Context.ExecuteAsync<CreateTagRequest, CreateTagResponse>(_context, request);
        
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

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<CreateTagRequest, CreateTagResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}
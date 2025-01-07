using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Tags.Create.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsCreateUseCaseTests : UseCaseTestBase
{
    private const string TagName = "tag";
    
    private readonly UseCaseExecutionContext _context;
    
    public TagsCreateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();

        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateTagRequest(TagName);

        var response = await Services.RequestExecutor.ExecuteAsync<CreateTagRequest, CreateTagResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Tag);
        Assert.Equal(TagName, response.Tag.Name);
    }
}
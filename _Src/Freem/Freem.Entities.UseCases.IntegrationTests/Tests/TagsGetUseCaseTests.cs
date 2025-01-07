using Freem.Entities.Tags;
using Freem.Entities.Tags.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Tags.Get.Models;

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
        Assert.True(response.Founded);
        Assert.NotNull(response.Tag);
        
        Assert.Equal(_tag, response.Tag, new TagEqualityComparer());
    }
}
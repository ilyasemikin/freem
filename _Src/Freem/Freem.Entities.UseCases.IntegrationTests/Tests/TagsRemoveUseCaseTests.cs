using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Tags.Remove.Models;

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

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}
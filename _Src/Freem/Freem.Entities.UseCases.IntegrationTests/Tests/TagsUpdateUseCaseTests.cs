using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Models.Fields;
using Freem.Entities.UseCases.Tags.Update.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class TagsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "name";
    
    private readonly UseCaseExecutionContext _context;
    private readonly TagIdentifier _tagId;
    
    public TagsUpdateUseCaseTests(ServicesContext services) : base(services)
    {
        var userId = services.Samples.Users.Register();
        var tag = services.Samples.Tags.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _tagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateTagRequest(_tagId)
        {
            Name = new UpdateField<TagName>(UpdatedName)
        };

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}
using Freem.Domain.Tags;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Tags.Base;

public abstract class TagTestBase : TestBase
{
    private const int TagsCount = 3;
    
    protected IReadOnlyList<TagId> AddedTagIds { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();

        AddedTagIds = await Context.Preparer.Tags.CreateManyAsync(TagsCount);
    }
}
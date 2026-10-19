using Freem.Domain.Tags;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Tags.Base;

public abstract class TagTestBase : TestBase
{
    private const int TagsCount = 3;
    
    protected IReadOnlyList<TagId> AddedTagIds { get; }
    
    public TagTestBase(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();

        
        AddedTagIds = Context.Preparer.Tags
            .CreateMany(TagsCount)
            .ToArray();
    }
}
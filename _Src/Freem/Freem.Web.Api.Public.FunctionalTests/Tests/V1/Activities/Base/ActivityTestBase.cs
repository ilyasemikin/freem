using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Relations.Collections;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Activities.Base;

public abstract class ActivityTestBase : TestBase
{
    private const int ActivitiesCount = 3;
    
    protected IReadOnlyList<ActivityIdentifier> AddedActivityIds { get; }
    
    protected RelatedTagsIdentifiersCollection AddedRelatedTags { get; }
    
    protected ActivityTestBase(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();

        var tagId = Context.Preparer.Tags.Create();
        AddedRelatedTags = new RelatedTagsIdentifiersCollection([tagId]);
        
        AddedActivityIds = Context.Preparer.Activities
            .CreateMany(ActivitiesCount)
            .ToArray();
    }
}
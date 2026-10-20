using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Activities.Base;

public abstract class ActivityTestBase : TestBase
{
    private const int ActivitiesCount = 3;
    
    protected IReadOnlyList<ActivityId> AddedActivityIds { get; private set; } = null!;
    
    protected RelatedTagIdsCollection AddedRelatedTags { get; private set; } = null!;
    
    protected ActivityTestBase(TestContext context, ITestOutputHelper output) 
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();

        var tagId = await Context.Preparer.Tags.CreateAsync();
        AddedRelatedTags = new RelatedTagIdsCollection([tagId]);
        
        AddedActivityIds = await Context.Preparer.Activities.CreateManyAsync(ActivitiesCount);
    }
}
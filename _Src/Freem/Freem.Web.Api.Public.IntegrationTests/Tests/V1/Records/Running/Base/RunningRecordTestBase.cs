using Freem.Domain.Relations.Collections;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;

public abstract class RunningRecordTestBase : TestBase
{
    protected RelatedActivityIdsCollection AddedRelatedActivities { get; private set; } = null!;
    protected RelatedTagIdsCollection AddedRelatedTags { get; private set; } = null!;
    
    protected RunningRecordTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.RegisterAsync();

        var tagId = await Context.Preparer.Tags.CreateAsync();
        var activityId = await Context.Preparer.Activities.CreateAsync();

        AddedRelatedTags = new RelatedTagIdsCollection([tagId]);
        AddedRelatedActivities = new RelatedActivityIdsCollection([activityId]);
    }
}
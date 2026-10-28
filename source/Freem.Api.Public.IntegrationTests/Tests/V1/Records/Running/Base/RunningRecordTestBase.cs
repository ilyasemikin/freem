using Freem.Domain.Relations.Collections;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Running.Base;

public abstract class RunningRecordTestBase : TestBase
{
    protected RelatedActivityIdsCollection AddedRelatedActivities { get; private set; } = null!;
    protected RelatedTagIdsCollection AddedRelatedTags { get; private set; } = null!;

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
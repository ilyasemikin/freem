using Freem.Domain.Relations.Collections;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running.Base;

public abstract class RunningRecordTestBase : TestBase
{
    protected RelatedActivityIdsCollection AddedRelatedActivities { get; }
    protected RelatedTagIdsCollection AddedRelatedTags { get; }
    
    protected RunningRecordTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();

        var tagId = Context.Preparer.Tags.Create();
        var activityId = Context.Preparer.Activities.Create();

        AddedRelatedTags = new RelatedTagIdsCollection([tagId]);
        AddedRelatedActivities = new RelatedActivityIdsCollection([activityId]);
    }
}
using Freem.Entities.Relations.Collections;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Running.Base;

public abstract class RunningRecordTestBase : TestBase
{
    protected RelatedActivitiesCollection AddedRelatedActivities { get; }
    protected RelatedTagsCollection AddedRelatedTags { get; }
    
    protected RunningRecordTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();
        Context.Preparer.Users.Login();

        var tagId = Context.Preparer.Tags.Create();
        var activityId = Context.Preparer.Activities.Create();

        AddedRelatedTags = new RelatedTagsCollection([tagId]);
        AddedRelatedActivities = new RelatedActivitiesCollection([activityId]);
    }
}
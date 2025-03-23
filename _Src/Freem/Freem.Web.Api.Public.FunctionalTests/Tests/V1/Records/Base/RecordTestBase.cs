using Freem.Entities.Records.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Time.Models;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Records.Base;

public abstract class RecordTestBase : TestBase
{
    private const int RecordsCount = 3;
    
    protected IReadOnlyList<DateTimePeriod> AddedPeriods { get; }
    protected IReadOnlyList<RecordIdentifier> AddedRecordIds { get; }
    
    protected RelatedActivitiesCollection AddedRelatedActivities { get; }
    protected RelatedTagsCollection AddedRelatedTags { get; }
    
    protected RecordTestBase(TestContext context, ITestOutputHelper? output = null) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();
        Context.Preparer.Users.Login();

        var tagId = Context.Preparer.Tags.Create();
        var activityId = Context.Preparer.Activities.Create();
        
        AddedRelatedTags = new RelatedTagsCollection([tagId]);
        AddedRelatedActivities = new RelatedActivitiesCollection([activityId]);

        var periods = new DateTimePeriod[RecordsCount];

        var startAt = DateTimeOffset.UtcNow.AddDays(-1);
        for (var i = 0; i < RecordsCount; i++)
        {
            var endAt = startAt + TimeSpan.FromHours(i);
            var period = new DateTimePeriod(startAt, endAt);
            startAt = endAt;

            periods[i] = period;
        }
        
        AddedPeriods = periods;
        AddedRecordIds = Context.Preparer.Records
            .CreateMany(periods, AddedRelatedActivities, AddedRelatedTags)
            .ToArray();
    }
}
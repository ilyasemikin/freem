using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Time.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Records.Base;

public abstract class RecordTestBase : TestBase
{
    private const int RecordsCount = 3;
    
    protected IReadOnlyList<DateTimePeriod> AddedPeriods { get; private set; } = null!;
    protected IReadOnlyList<RecordId> AddedRecordIds { get; private set; } = null!;
    
    protected RelatedActivityIdsCollection AddedRelatedActivities { get; private set; } = null!;
    protected RelatedTagIdsCollection AddedRelatedTags { get; private set; } = null!;
    
    protected RecordTestBase(TestContext context, ITestOutputHelper? output = null) 
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
        AddedRecordIds = await Context.Preparer.Records.CreateManyAsync(periods, AddedRelatedActivities, AddedRelatedTags);
    }
}
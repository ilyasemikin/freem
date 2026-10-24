using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Records.Running;
using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Time;

namespace Freem.Api.Public.IntegrationTests.Context.Preparers;

public sealed class RecordsPreparer
{
    private readonly TestContext _context;

    public RecordsPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public async Task<IReadOnlyList<RecordId>> CreateManyAsync(
        IEnumerable<DateTimePeriod> periods,
        IReadOnlyRelatedIdsCollection<ActivityId> activities,
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        tags ??= RelatedTagIdsCollection.Empty;
        
        var ids = new List<RecordId>();
        foreach (var period in periods)
        {
            var periodRequest = new RecordPeriodItem(period);
            var request = new RecordCreateRequest([periodRequest], activities, tags);
            var response = await _context.Client.Records.CreateAsync(request);

            ids.Add(response.Unwrap().Id);
        }

        return ids;
    }

    public async Task StartRunningAsync(
        DateTimeOffset? startAt, 
        RelatedActivityIdsCollection activities, 
        RelatedTagIdsCollection? tags = null)
    {
        tags ??= RelatedTagIdsCollection.Empty;
        
        var request = new RunningRecordStartRequest(startAt, activities, tags);
        var response = await _context.Client.Records.StartAsync(request);
        response.EnsureSuccess();
    }

    public async Task StopRunningAsync()
    {
        var request = new RunningRecordStopRequest();
        var response = await _context.Client.Records.StopAsync(request);
        response.EnsureSuccess();
    }
}

using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;
using Freem.Api.Public.Contracts.DTO.Records;
using Freem.Api.Public.Contracts.DTO.Records.Running;
using Freem.Api.Public.IntegrationTests.Extensions;

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
            var periodRequest = new RecordPeriodRequest(period);
            var request = new CreateRecordRequest([periodRequest], activities, tags);
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
        
        var request = new StartRunningRecordRequest(startAt, activities, tags);
        var response = await _context.Client.Records.StartAsync(request);
        response.EnsureSuccess();
    }

    public async Task StopRunningAsync()
    {
        var request = new StopRunningRecordRequest();
        var response = await _context.Client.Records.StopAsync(request);
        response.EnsureSuccess();
    }
}

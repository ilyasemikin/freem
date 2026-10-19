using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.Contracts.DTO.Records.Running;

namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

public sealed class RecordsPreparer
{
    private readonly TestContext _context;

    public RecordsPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public IEnumerable<RecordId> CreateMany(
        IEnumerable<DateTimePeriod> periods,
        IReadOnlyRelatedIdsCollection<ActivityId> activities,
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        tags ??= RelatedTagIdsCollection.Empty;
        
        var ids = new List<RecordId>();
        foreach (var period in periods)
        {
            var request = new CreateRecordRequest(period, activities, tags);
            var response = _context.SyncClient.Records.Create(request);
            response.EnsureSuccess();

            ids.Add(response.Value.Id);
        }

        return ids;
    }

    public void StartRunning(
        DateTimeOffset? startAt, 
        RelatedActivityIdsCollection activities, 
        RelatedTagIdsCollection? tags = null)
    {
        tags ??= RelatedTagIdsCollection.Empty;
        
        var request = new StartRunningRecordRequest(startAt, activities, tags);
        var response = _context.SyncClient.Records.Start(request);
        response.EnsureSuccess();
    }
    
    public void StopRunning()
    {
        var request = new StopRunningRecordRequest();
        var response = _context.SyncClient.Records.Stop(request);
        response.EnsureSuccess();
    }
}
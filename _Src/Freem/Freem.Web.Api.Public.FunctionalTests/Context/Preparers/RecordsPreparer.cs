using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
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

    public IEnumerable<RecordIdentifier> CreateMany(
        IEnumerable<DateTimePeriod> periods,
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> activities,
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        tags ??= RelatedTagsIdentifiersCollection.Empty;
        
        var ids = new List<RecordIdentifier>();
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
        RelatedActivitiesIdentifiersCollection activities, 
        RelatedTagsIdentifiersCollection? tags = null)
    {
        tags ??= RelatedTagsIdentifiersCollection.Empty;
        
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
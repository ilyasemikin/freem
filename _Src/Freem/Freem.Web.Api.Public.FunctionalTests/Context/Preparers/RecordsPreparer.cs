using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
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
        RelatedActivitiesCollection activities, 
        RelatedTagsCollection? tags = null)
    {
        tags ??= RelatedTagsCollection.Empty;
        
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
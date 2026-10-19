using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Web.Api.Public.Contracts.DTO.Activities;

namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

public sealed class ActivitiesPreparer
{
    private readonly TestContext _context;

    public ActivitiesPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public ActivityId Create(string name = "name0")
    {
        var activityName = ActivityName.Parse(name, null);
        var request = new CreateActivityRequest(activityName);
        var response = _context.SyncClient.Activities.Create(request);
        response.EnsureSuccess();

        return response.Value.Id;
    }

    public IEnumerable<ActivityId> CreateMany(
        int count, 
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        tags ??= RelatedTagIdsCollection.Empty;

        var ids = new ActivityId[count];
        for (var i = 0; i < count; i++)
        {
            var name = $"name{i}";
            var activityName = ActivityName.Parse(name, null);
            
            var request = new CreateActivityRequest(activityName, tags);
            var response = _context.SyncClient.Activities.Create(request);
            response.EnsureSuccess();

            ids[i] = response.Value.Id;
        }

        return ids;
    }

    public void Archive(ActivityId id)
    {
        var response = _context.SyncClient.Activities.Archive(id);
        response.EnsureSuccess();
    }
}
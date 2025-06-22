using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
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

    public ActivityIdentifier Create(string name = "name0")
    {
        var request = new CreateActivityRequest(name);
        var response = _context.SyncClient.Activities.Create(request);
        response.EnsureSuccess();

        return response.Value.Id;
    }

    public IEnumerable<ActivityIdentifier> CreateMany(
        int count, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        tags ??= RelatedTagsIdentifiersCollection.Empty;

        var ids = new ActivityIdentifier[count];
        for (var i = 0; i < count; i++)
        {
            var name = $"name{i}";
            
            var request = new CreateActivityRequest(name, tags);
            var response = _context.SyncClient.Activities.Create(request);
            response.EnsureSuccess();

            ids[i] = response.Value.Id;
        }

        return ids;
    }

    public void Archive(ActivityIdentifier id)
    {
        var response = _context.SyncClient.Activities.Archive(id);
        response.EnsureSuccess();
    }
}
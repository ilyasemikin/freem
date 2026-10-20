using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.IntegrationTests.Extensions;

namespace Freem.Web.Api.Public.IntegrationTests.Context.Preparers;

public sealed class ActivitiesPreparer
{
    private readonly TestContext _context;

    public ActivitiesPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public async Task<ActivityId> CreateAsync(string name = "name0")
    {
        var activityName = ActivityName.Parse(name, null);
        var request = new CreateActivityRequest(activityName);
        var response = await _context.Client.Activities.CreateAsync(request);

        return response.Unwrap().Id;
    }

    public async Task<IReadOnlyList<ActivityId>> CreateManyAsync(
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
            var response = await _context.Client.Activities.CreateAsync(request);

            ids[i] = response.Unwrap().Id;
        }

        return ids;
    }

    public async Task ArchiveAsync(ActivityId id)
    {
        var response = await _context.Client.Activities.ArchiveAsync(id);
        response.EnsureSuccess();
    }
}

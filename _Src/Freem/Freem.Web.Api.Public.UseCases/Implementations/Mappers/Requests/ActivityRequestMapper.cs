using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Entities.UseCases.Contracts.Activities.Find;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Entities.UseCases.Contracts.Activities.Unarchive;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Web.Api.Public.UseCases.Contracts.Activities;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class ActivityRequestMapper
{
    public static ArchiveActivityRequest Map(this ApiArchiveActivityRequest request)
    {
        return new ArchiveActivityRequest(request.Id);
    }

    public static CreateActivityRequest Map(this ApiCreateActivityRequest request)
    {
        var tags = new RelatedTagIdsCollection(request.Body.Tags);
        return new CreateActivityRequest(request.Body.Name)
        {
            Tags = tags
        };
    }

    public static FindActivityRequest Map(this ApiFindActivityByNameRequest request)
    {
        return new FindActivityRequest(request.Query.SearchText);
    }

    public static GetActivityRequest Map(this ApiGetActivityRequest request)
    {
        return new GetActivityRequest(request.Id);
    }

    public static ListActivityRequest Map(this ApiListActivityRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListActivityRequest(limit, offset);
    }

    public static RemoveActivityRequest Map(this ApiRemoveActivityRequest request)
    {
        return new RemoveActivityRequest(request.Id);
    }

    public static UnarchiveActivityRequest Map(this ApiUnarchiveActivityRequest request)
    {
        return new UnarchiveActivityRequest(request.Id);
    }

    public static UpdateActivityRequest Map(this ApiUpdateActivityRequest request)
    {
        var body = request.Body;
        UpdateField<RelatedTagIdsCollection>? tags = null;
        if (body.Tags is not null)
        {
            var collection = new RelatedTagIdsCollection(body.Tags.Value);
            tags = new UpdateField<RelatedTagIdsCollection>(collection);
        }

        return new UpdateActivityRequest(request.Id)
        {
            Name = body.Name?.Map(),
            Tags = tags
        };
    }
}

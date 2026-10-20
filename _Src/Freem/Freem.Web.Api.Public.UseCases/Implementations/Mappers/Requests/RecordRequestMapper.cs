using Freem.Domain.Relations.Collections;
using Freem.Entities.UseCases.Contracts;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Web.Api.Public.UseCases.Contracts.Records;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class RecordRequestMapper
{
    public static CreateRecordRequest Map(this ApiCreateRecordRequest request)
    {
        var body = request.Body;
        var activities = new RelatedActivityIdsCollection(body.Activities);
        var tags = new RelatedTagIdsCollection(body.Tags);
        return new CreateRecordRequest(body.Period, activities)
        {
            Name = body.Name,
            Description = body.Description,
            Tags = tags
        };
    }

    public static GetRecordRequest Map(this ApiGetRecordRequest request)
    {
        return new GetRecordRequest(request.Id);
    }

    public static ListRecordRequest Map(this ApiListRecordRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListRecordRequest(limit, offset);
    }

    public static PeriodListRequest Map(this ApiListRecordByPeriodRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        return new PeriodListRequest(request.Query.Period, limit);
    }

    public static RemoveRecordRequest Map(this ApiRemoveRecordRequest request)
    {
        return new RemoveRecordRequest(request.Id);
    }

    public static UpdateRecordRequest Map(this ApiUpdateRecordRequest request)
    {
        var body = request.Body;

        UpdateField<RelatedActivityIdsCollection>? activities = null;
        if (body.Activities is not null)
        {
            var collection = new RelatedActivityIdsCollection(body.Activities.Value);
            activities = new UpdateField<RelatedActivityIdsCollection>(collection);
        }

        UpdateField<RelatedTagIdsCollection>? tags = null;
        if (body.Tags?.Value is not null)
        {
            var collection = new RelatedTagIdsCollection(body.Tags.Value);
            tags = new UpdateField<RelatedTagIdsCollection>(collection);
        }

        return new UpdateRecordRequest(request.Id)
        {
            Period = body.Period?.Map(),
            Name = body.Name?.Map(),
            Description = body.Description?.Map(),
            Activities = activities,
            Tags = tags
        };
    }
}

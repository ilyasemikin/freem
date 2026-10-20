using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Records.Get;
using Freem.Domain.UseCases.Contracts.Records.List;
using Freem.Domain.UseCases.Contracts.Records.PeriodList;
using Freem.Domain.UseCases.Contracts.Records.Remove;
using Freem.Domain.UseCases.Contracts.Records.RemovePeriod;
using Freem.Domain.UseCases.Contracts.Records.Update;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Api.Public.UseCases.Contracts.Records;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class RecordRequestMapper
{
    public static CreateRecordRequest Map(this ApiCreateRecordRequest request)
    {
        var body = request.Body;
        var activities = new RelatedActivityIdsCollection(body.Activities);
        var tags = new RelatedTagIdsCollection(body.Tags);
        var periods = body.Periods
            .Select(p => new NewRecordPeriod(p.Period) { Description = p.Description })
            .ToArray();
        return new CreateRecordRequest(periods, activities)
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

    public static AddRecordPeriodRequest Map(this ApiAddRecordPeriodRequest request)
    {
        var body = request.Body;
        return new AddRecordPeriodRequest(request.RecordId, body.Period)
        {
            Description = body.Description
        };
    }

    public static UpdateRecordPeriodRequest Map(this ApiUpdateRecordPeriodRequest request)
    {
        var body = request.Body;
        return new UpdateRecordPeriodRequest(request.RecordId, request.PeriodId)
        {
            Period = body.Period?.Map(),
            Description = body.Description?.Map()
        };
    }

    public static RemoveRecordPeriodRequest Map(this ApiRemoveRecordPeriodRequest request)
    {
        return new RemoveRecordPeriodRequest(request.RecordId, request.PeriodId);
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
            Name = body.Name?.Map(),
            Description = body.Description?.Map(),
            Activities = activities,
            Tags = tags
        };
    }
}

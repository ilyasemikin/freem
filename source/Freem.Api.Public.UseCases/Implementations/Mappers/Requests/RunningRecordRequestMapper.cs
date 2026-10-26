using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.RunningRecords.Get;
using Freem.Domain.UseCases.Contracts.RunningRecords.Remove;
using Freem.Domain.UseCases.Contracts.RunningRecords.Start;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Domain.UseCases.Contracts.RunningRecords.Update;
using Freem.Api.Public.UseCases.Contracts.RunningRecords;
using Utilities.Clock.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class RunningRecordRequestMapper
{
    public static GetRunningRecordRequest Map(this ApiGetRunningRecordRequest request)
    {
        return GetRunningRecordRequest.Instance;
    }

    public static RemoveRunningRecordRequest Map(this ApiRemoveRunningRecordRequest request)
    {
        return RemoveRunningRecordRequest.Instance;
    }

    public static StartRunningRecordRequest Map(this ApiStartRunningRecordRequest request, IClock clock)
    {
        var body = request.Body;
        var startAt = body.StartAt ?? clock.Get();
        var activities = new RelatedActivityIdsCollection(body.Activities);
        var tags = new RelatedTagIdsCollection(body.Tags);
        return new StartRunningRecordRequest(startAt, activities)
        {
            Name = body.Name,
            Description = body.Description,
            Tags = tags
        };
    }

    public static StopRunningRecordRequest Map(this ApiStopRunningRecordRequest request, IClock clock)
    {
        var endAt = request.Body.EndAt ?? clock.Get();
        return new StopRunningRecordRequest(endAt);
    }

    public static UpdateRunningRecordRequest Map(this ApiUpdateRunningRecordRequest request)
    {
        var body = request.Body;

        UpdateField<RelatedActivityIdsCollection>? activities = null;
        if (body.Activities is not null)
        {
            var collection = new RelatedActivityIdsCollection(body.Activities.Value);
            activities = new UpdateField<RelatedActivityIdsCollection>(collection);
        }

        UpdateField<RelatedTagIdsCollection>? tags = null;
        if (body.Tags is not null)
        {
            var collection = new RelatedTagIdsCollection(body.Tags.Value);
            tags = new UpdateField<RelatedTagIdsCollection>(collection);
        }

        return new UpdateRunningRecordRequest
        {
            Name = body.Name?.Map(),
            Description = body.Description?.Map(),
            Activities = activities,
            Tags = tags,
            StartAt = body.StartAt?.Map()
        };
    }
}

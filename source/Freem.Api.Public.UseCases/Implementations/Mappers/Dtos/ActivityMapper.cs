using Freem.Api.Public.Contracts.V1.Activities.Dtos;
using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Domain.Entities.Activities;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class ActivityMapper
{
    public static ActivityDto Map(this Activity activity)
    {
        return new ActivityDto(activity.Id, activity.Name, activity.Color, activity.Status, activity.Tags);
    }
}

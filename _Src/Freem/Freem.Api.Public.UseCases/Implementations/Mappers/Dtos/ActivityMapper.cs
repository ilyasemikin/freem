using Freem.Domain.Entities.Activities;
using Freem.Api.Public.Contracts.DTO.Activities;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class ActivityMapper
{
    public static ActivityResponse Map(this Activity activity)
    {
        return new ActivityResponse(activity.Id, activity.Name, activity.Color, activity.Status, activity.Tags);
    }
}

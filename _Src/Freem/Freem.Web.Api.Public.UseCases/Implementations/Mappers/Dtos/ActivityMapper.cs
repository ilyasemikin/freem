using Freem.Domain.Entities.Activities;
using Freem.Web.Api.Public.Contracts.DTO.Activities;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class ActivityMapper
{
    public static ActivityResponse Map(this Activity activity)
    {
        return new ActivityResponse(activity.Id, activity.Name, activity.Status, activity.Tags);
    }
}

using Freem.Domain.Entities.Activities;

namespace Freem.Entities.UseCases.Contracts.Activities.Get;

public sealed class GetActivityResponse
{
    public Activity Activity { get; }

    public GetActivityResponse(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        Activity = activity;
    }
}
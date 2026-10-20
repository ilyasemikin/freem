using Freem.Domain.Entities.Activities;

namespace Freem.Entities.UseCases.Contracts.Activities.Create;

public sealed class CreateActivityResponse
{
    public Activity Activity { get; }

    public CreateActivityResponse(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        Activity = activity;
    }
}
using Freem.Entities.Activities;

namespace Freem.Entities.UseCases.Activities.Create.Models;

public sealed class CreateActivityResponse
{
    public Activity Activity { get; }

    public CreateActivityResponse(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);
        
        Activity = activity;
    }
}
using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities;

namespace Freem.Entities.UseCases.Activities.Get.Models;

public sealed class GetActivityResponse
{
    [MemberNotNullWhen(true, nameof(Activity))]
    public bool Founded { get; }
    public Activity? Activity { get; }

    public GetActivityResponse(Activity? activity)
    {
        Founded = activity is not null;
        Activity = activity;
    }
}
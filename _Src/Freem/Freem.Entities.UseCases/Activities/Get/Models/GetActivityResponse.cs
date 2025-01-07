using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Activities.Get.Models;

public sealed class GetActivityResponse
{
    [MemberNotNullWhen(true, nameof(Activity))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Activity? Activity { get; }
    public Error<GetActivityErrorCode>? Error { get; }

    private GetActivityResponse(Activity? activity = null, Error<GetActivityErrorCode>? error = null)
    {
        Success = activity is not null;
        Activity = activity;
        Error = error;
    }

    public static GetActivityResponse CreateSuccess(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        return new GetActivityResponse(activity);
    }

    public static GetActivityResponse CreateFailure(GetActivityErrorCode code, string? message = null)
    {
        var error = new Error<GetActivityErrorCode>(code, message);
        return new GetActivityResponse(error: error);
    }

    internal static GetActivityResponse Create(SearchEntityResult<Activity> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetActivityErrorCode.ActivityNotFound);
    }
}
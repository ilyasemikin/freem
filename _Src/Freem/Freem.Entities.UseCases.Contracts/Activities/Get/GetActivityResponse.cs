using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Activities.Get;

public sealed class GetActivityResponse : IResponse<GetActivityErrorCode>
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

    public static GetActivityResponse CreateFailure(GetActivityErrorCode code)
    {
        var error = new Error<GetActivityErrorCode>(code);
        return new GetActivityResponse(error: error);
    }

    public static GetActivityResponse Create(SearchEntityResult<Activity> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetActivityErrorCode.ActivityNotFound);
    }
}
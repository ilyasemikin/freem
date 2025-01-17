using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Activities.Create;

public sealed class CreateActivityResponse : IResponse<CreateActivityErrorCode>
{
    [MemberNotNullWhen(true, nameof(Activity))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Activity? Activity { get; }
    public Error<CreateActivityErrorCode>? Error { get; }

    private CreateActivityResponse(Activity? activity = null, Error<CreateActivityErrorCode>? error = null)
    {
        Success = activity is not null;
        Activity = activity;
        Error = error;
    }

    public static CreateActivityResponse CreateSuccess(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);
        
        return new CreateActivityResponse(activity);
    }

    public static CreateActivityResponse CreateFailure(CreateActivityErrorCode code, string? message = null)
    {
        var error = new Error<CreateActivityErrorCode>(code, message);

        return new CreateActivityResponse(error: error);
    }
}
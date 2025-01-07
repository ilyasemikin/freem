using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Activities.Update.Models;

public sealed class UpdateActivityResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<UpdateActivityErrorCode>? Error { get; }

    private UpdateActivityResponse(Error<UpdateActivityErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UpdateActivityResponse CreateSuccess()
    {
        return new UpdateActivityResponse();
    }

    public static UpdateActivityResponse CreateFailure(UpdateActivityErrorCode code, string? message = null)
    {
        var error = new Error<UpdateActivityErrorCode>(code, message);
        return new UpdateActivityResponse(error);
    }
}
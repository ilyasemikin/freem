using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Activities.Remove.Models;

public sealed class RemoveActivityResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<RemoveActivityErrorCode>? Error { get; }

    private RemoveActivityResponse(Error<RemoveActivityErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static RemoveActivityResponse CreateSuccess()
    {
        return new RemoveActivityResponse();
    }

    public static RemoveActivityResponse CreateFailure(RemoveActivityErrorCode code, string? message = null)
    {
        var error = new Error<RemoveActivityErrorCode>(code, message);
        return new RemoveActivityResponse(error);
    }
}
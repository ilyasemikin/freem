using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.Remove;

public sealed class RemoveTagResponse : IResponse<RemoveTagErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<RemoveTagErrorCode>? Error { get; }

    private RemoveTagResponse(Error<RemoveTagErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static RemoveTagResponse CreateSuccess()
    {
        return new RemoveTagResponse();
    }

    public static RemoveTagResponse CreateFailure(RemoveTagErrorCode code, string? message = null)
    {
        var error = new Error<RemoveTagErrorCode>(code, message);
        return new RemoveTagResponse(error);
    }
}
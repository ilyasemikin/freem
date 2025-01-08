using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Tags.Remove.Models;

public sealed class RemoveTagResponse
{
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
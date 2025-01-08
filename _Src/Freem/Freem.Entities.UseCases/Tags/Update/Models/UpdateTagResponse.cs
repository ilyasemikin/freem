using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Tags.Update.Models;

public sealed class UpdateTagResponse
{
    public bool Success { get; }
    
    public Error<UpdateTagErrorCode>? Error { get; }

    private UpdateTagResponse(Error<UpdateTagErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UpdateTagResponse CreateSuccess()
    {
        return new UpdateTagResponse();
    }

    public static UpdateTagResponse CreateFailure(UpdateTagErrorCode code, string? message = null)
    {
        var error = new Error<UpdateTagErrorCode>(code, message);
        return new UpdateTagResponse(error);
    }
}
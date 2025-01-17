using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Tags.Update;

public sealed class UpdateTagResponse : IResponse<UpdateTagErrorCode>
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
using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Tags.Update;

public sealed class UpdateTagResponse : IResponse<UpdateTagErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
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
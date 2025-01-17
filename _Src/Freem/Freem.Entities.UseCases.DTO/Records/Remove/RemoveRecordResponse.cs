using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Records.Remove;

public sealed class RemoveRecordResponse : IResponse<RemoveRecordErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<RemoveRecordErrorCode>? Error { get; }

    private RemoveRecordResponse(Error<RemoveRecordErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static RemoveRecordResponse CreateSuccess()
    {
        return new RemoveRecordResponse();
    }

    public static RemoveRecordResponse CreateFailure(RemoveRecordErrorCode code, string? message = null)
    {
        var error = new Error<RemoveRecordErrorCode>(code, message);
        return new RemoveRecordResponse(error);
    }
}
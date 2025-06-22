using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Records.Remove;

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

    public static RemoveRecordResponse CreateFailure(RemoveRecordErrorCode code)
    {
        var error = new Error<RemoveRecordErrorCode>(code);
        return new RemoveRecordResponse(error);
    }
}
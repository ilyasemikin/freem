using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Remove;

public sealed class RemoveRunningRecordResponse : IResponse<RemoveRunningRecordErrorCode>
{
    [MemberNotNullWhen(true)]
    public bool Success { get; }
    
    public Error<RemoveRunningRecordErrorCode>? Error { get; }

    private RemoveRunningRecordResponse(Error<RemoveRunningRecordErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static RemoveRunningRecordResponse CreateSuccess()
    {
        return new RemoveRunningRecordResponse();
    }

    public static RemoveRunningRecordResponse CreateFailure(RemoveRunningRecordErrorCode code, string? message = null)
    {
        var error = new Error<RemoveRunningRecordErrorCode>(code, message);
        return new RemoveRunningRecordResponse(error);
    }
}
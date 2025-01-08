using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.RunningRecords.Stop.Models;

public sealed class StopRunningRecordResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<StopRunningRecordErrorCode>? Error { get; }
    
    public StopRunningRecordResponse(Error<StopRunningRecordErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static StopRunningRecordResponse CreateSuccess()
    {
        return new StopRunningRecordResponse();
    }

    public static StopRunningRecordResponse CreateFailure(StopRunningRecordErrorCode code, string? message = null)
    {
        var error = new Error<StopRunningRecordErrorCode>(code, message);
        return new StopRunningRecordResponse(error);
    }
}
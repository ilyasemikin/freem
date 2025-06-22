using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Stop;

public sealed class StopRunningRecordResponse : IResponse<StopRunningRecordErrorCode>
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
        var error = new Error<StopRunningRecordErrorCode>(code);
        return new StopRunningRecordResponse(error);
    }
}
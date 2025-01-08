using System.Diagnostics.CodeAnalysis;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.RunningRecords.Start.Models;

public sealed class StartRunningRecordResponse
{
    [MemberNotNullWhen(true, nameof(RunningRecord))]
    public bool Success { get; }
    
    public RunningRecord? RunningRecord { get; }
    public Error<StartRunningRecordErrorCode>? Error { get; }

    private StartRunningRecordResponse(RunningRecord? record = null, Error<StartRunningRecordErrorCode>? error = null)
    {
        Success = record is not null;
        RunningRecord = record;
        Error = error;
    }

    public static StartRunningRecordResponse CreateSuccess(RunningRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        
        return new StartRunningRecordResponse(record);
    }

    public static StartRunningRecordResponse CreateFailure(StartRunningRecordErrorCode code, string? message = null)
    {
        var error = new Error<StartRunningRecordErrorCode>(code, message);
        return new StartRunningRecordResponse(error: error);
    }
}
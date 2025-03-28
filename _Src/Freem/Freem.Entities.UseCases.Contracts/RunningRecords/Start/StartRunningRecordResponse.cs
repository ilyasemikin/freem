﻿using System.Diagnostics.CodeAnalysis;
using Freem.Entities.RunningRecords;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Start;

public sealed class StartRunningRecordResponse : IResponse<StartRunningRecordErrorCode>
{
    [MemberNotNullWhen(true, nameof(RunningRecord))]
    [MemberNotNullWhen(false, nameof(Error))]
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
using System.Diagnostics.CodeAnalysis;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Get;

public sealed class GetRunningRecordResponse : IResponse<GetRunningRecordErrorCode>
{
    [MemberNotNullWhen(true, nameof(Record))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public RunningRecord? Record { get; }
    public Error<GetRunningRecordErrorCode>? Error { get; }

    private GetRunningRecordResponse(RunningRecord? record = null, Error<GetRunningRecordErrorCode>? error = null)
    {
        Success = record is not null;
        Record = record;
        Error = error;
    }

    public static GetRunningRecordResponse CreateSuccess(RunningRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        
        return new GetRunningRecordResponse(record);
    }

    public static GetRunningRecordResponse CreateFailure(GetRunningRecordErrorCode code)
    {
        var error = new Error<GetRunningRecordErrorCode>(code);
        return new GetRunningRecordResponse(error: error);
    }

    public static GetRunningRecordResponse Create(SearchEntityResult<RunningRecord> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetRunningRecordErrorCode.RunningRecordNotFound);
    }
}
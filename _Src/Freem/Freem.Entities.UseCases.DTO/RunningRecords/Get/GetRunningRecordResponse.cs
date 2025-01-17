using System.Diagnostics.CodeAnalysis;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.RunningRecords.Get;

public sealed class GetRunningRecordResponse : IResponse<GetRunningRecordErrorCode>
{
    [MemberNotNullWhen(true, nameof(Record))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public RunningRecord? Record { get; }
    public Error<GetRunningRecordErrorCode>? Error { get; }

    public GetRunningRecordResponse(RunningRecord? record = null, Error<GetRunningRecordErrorCode>? error = null)
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

    public static GetRunningRecordResponse CreateFailure(GetRunningRecordErrorCode code, string? message = null)
    {
        var error = new Error<GetRunningRecordErrorCode>(code, message);
        return new GetRunningRecordResponse(error: error);
    }
}
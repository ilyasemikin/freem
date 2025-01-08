using System.Diagnostics.CodeAnalysis;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Abstractions.Models.Errors;
using Freem.Entities.UseCases.Records.Get.Models;

namespace Freem.Entities.UseCases.RunningRecords.Get.Models;

public sealed class GetRunningRecordResponse
{
    [MemberNotNullWhen(true, nameof(Record))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public RunningRecord? Record { get; }
    public Error<GetRecordErrorCode>? Error { get; }

    public GetRunningRecordResponse(RunningRecord? record = null, Error<GetRecordErrorCode>? error = null)
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

    public static GetRunningRecordResponse CreateFailure(GetRecordErrorCode code, string? message = null)
    {
        var error = new Error<GetRecordErrorCode>(code, message);
        return new GetRunningRecordResponse(error: error);
    }
}
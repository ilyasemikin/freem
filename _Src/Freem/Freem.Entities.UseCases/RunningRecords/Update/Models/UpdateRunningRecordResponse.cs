using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.RunningRecords.Update.Models;

public sealed class UpdateRunningRecordResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<UpdateRunningRecordErrorCode>? Error { get; }

    private UpdateRunningRecordResponse(Error<UpdateRunningRecordErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UpdateRunningRecordResponse CreateSuccess()
    {
        return new UpdateRunningRecordResponse();
    }

    public static UpdateRunningRecordResponse CreateFailure(UpdateRunningRecordErrorCode code)
    {
        var error = new Error<UpdateRunningRecordErrorCode>(code);
        return new UpdateRunningRecordResponse(error);
    }
}
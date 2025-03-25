using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.RunningRecords.Update;

public sealed class UpdateRunningRecordResponse : IResponse<UpdateRunningRecordErrorCode>
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
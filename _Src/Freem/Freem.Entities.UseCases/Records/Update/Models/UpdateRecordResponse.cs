using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Records.Update.Models;

public class UpdateRecordResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public Error<UpdateRecordErrorCode>? Error { get; }

    private UpdateRecordResponse(Error<UpdateRecordErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UpdateRecordResponse CreateSuccess()
    {
        return new UpdateRecordResponse();
    }

    public static UpdateRecordResponse CreateFailure(UpdateRecordErrorCode code, string? message = null)
    {
        var error = new Error<UpdateRecordErrorCode>(code, message);
        return new UpdateRecordResponse(error);
    }
}
using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Records.Update;

public class UpdateRecordResponse : IResponse<UpdateRecordErrorCode>
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

    public static UpdateRecordResponse CreateFailure(UpdateRecordErrorCode code)
    {
        var error = new Error<UpdateRecordErrorCode>(code);
        return new UpdateRecordResponse(error);
    }
}
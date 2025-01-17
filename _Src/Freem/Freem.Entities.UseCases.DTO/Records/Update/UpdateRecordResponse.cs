using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Records.Update;

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

    public static UpdateRecordResponse CreateFailure(UpdateRecordErrorCode code, string? message = null)
    {
        var error = new Error<UpdateRecordErrorCode>(code, message);
        return new UpdateRecordResponse(error);
    }
}
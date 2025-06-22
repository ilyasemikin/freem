using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Records;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Records.Create;

public sealed class CreateRecordResponse : IResponse<CreateRecordErrorCode>
{
    [MemberNotNullWhen(true, nameof(Record))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Record? Record { get; }
    public Error<CreateRecordErrorCode>? Error { get; }

    private CreateRecordResponse(Record? record = null, Error<CreateRecordErrorCode>? error = null)
    {
        Success = record is not null;
        Record = record;
        Error = error;
    }

    public static CreateRecordResponse CreateSuccess(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);
        
        return new CreateRecordResponse(record);
    }

    public static CreateRecordResponse CreateFailure(CreateRecordErrorCode code)
    {
        var error = new Error<CreateRecordErrorCode>(code);
        return new CreateRecordResponse(error: error);
    }
}
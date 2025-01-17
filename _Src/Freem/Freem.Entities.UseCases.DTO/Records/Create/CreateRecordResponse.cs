using Freem.Entities.Records;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Records.Create;

public sealed class CreateRecordResponse : IResponse<CreateRecordErrorCode>
{
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

    public static CreateRecordResponse CreateFailure(CreateRecordErrorCode code, string? message = null)
    {
        var error = new Error<CreateRecordErrorCode>(code, message);
        return new CreateRecordResponse(error: error);
    }
}
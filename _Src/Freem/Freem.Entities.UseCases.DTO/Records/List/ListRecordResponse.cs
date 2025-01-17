using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Records;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Filter;

namespace Freem.Entities.UseCases.DTO.Records.List;

public sealed class ListRecordResponse : IResponse<ListRecordErrorCode>
{
    [MemberNotNullWhen(true, nameof(Records))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyList<Record>? Records { get; }
    public TotalCount? TotalCount { get; }
    
    public Error<ListRecordErrorCode>? Error { get; }

    private ListRecordResponse(
        IReadOnlyList<Record>? records = null, 
        TotalCount? totalCount = null, 
        Error<ListRecordErrorCode>? error = null)
    {
        Success = error is null;
        Records = records;
        TotalCount = totalCount;
        Error = error;
    }

    public static ListRecordResponse CreateSuccess(IReadOnlyList<Record> records, TotalCount totalCount)
    {
        return new ListRecordResponse(records, totalCount);
    }

    public static ListRecordResponse CreateFailure(ListRecordErrorCode code, string? message = null)
    {
        var error = new Error<ListRecordErrorCode>(code, message);
        return new ListRecordResponse(error: error);
    }
}
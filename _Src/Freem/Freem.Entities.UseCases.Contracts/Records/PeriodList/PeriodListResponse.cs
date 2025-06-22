using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Records;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Records.PeriodList;

public sealed class PeriodListResponse : IResponse<PeriodListErrorCode>
{
    [MemberNotNullWhen(true, nameof(Records))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyList<Record>? Records { get; }
    public TotalCount? TotalCount { get; }
    
    public Error<PeriodListErrorCode>? Error { get; }

    private PeriodListResponse(
        IReadOnlyList<Record>? records = null,
        TotalCount? totalCount = null,
        Error<PeriodListErrorCode>? error = null)
    {
        Success = error is null;
        Records = records;
        TotalCount = totalCount;
        Error = error;
    }

    public static PeriodListResponse CreateSuccess(IReadOnlyList<Record> records, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(totalCount);

        return new PeriodListResponse(records, totalCount);
    }

    public static PeriodListResponse CreateFailure(PeriodListErrorCode code)
    {
        var error = new Error<PeriodListErrorCode>(code);
        return new PeriodListResponse(error: error);
    }
}
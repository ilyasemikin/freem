using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiRemoveRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.RemovePeriod;

    public RecordId RecordId { get; }
    public RecordPeriodId PeriodId { get; }

    public ApiRemoveRecordPeriodRequest(RecordId recordId, RecordPeriodId periodId)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(periodId);

        RecordId = recordId;
        PeriodId = periodId;
    }
}

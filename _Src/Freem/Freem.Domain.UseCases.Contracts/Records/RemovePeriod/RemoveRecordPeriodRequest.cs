using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.RemovePeriod;

public sealed class RemoveRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.RemovePeriod;

    public RecordId RecordId { get; }
    public RecordPeriodId PeriodId { get; }

    public RemoveRecordPeriodRequest(RecordId recordId, RecordPeriodId periodId)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(periodId);

        RecordId = recordId;
        PeriodId = periodId;
    }
}

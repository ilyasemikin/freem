using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Time.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;

public sealed class UpdateRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.UpdatePeriod;

    public RecordId RecordId { get; }
    public RecordPeriodId PeriodId { get; }

    public UpdateField<DateTimePeriod>? Period { get; init; }
    public UpdateField<RecordPeriodDescription?>? Description { get; init; }

    public UpdateRecordPeriodRequest(RecordId recordId, RecordPeriodId periodId)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(periodId);

        RecordId = recordId;
        PeriodId = periodId;
    }

    public bool HasChanges()
    {
        return
            Period is not null ||
            Description is not null;
    }
}

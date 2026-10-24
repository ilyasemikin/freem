using Freem.Api.Public.Contracts.V1.Records;
using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiUpdateRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.UpdatePeriod;

    public RecordId RecordId { get; }
    public RecordPeriodId PeriodId { get; }
    public RecordPeriodUpdateRequest Body { get; }

    public ApiUpdateRecordPeriodRequest(RecordId recordId, RecordPeriodId periodId, RecordPeriodUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(periodId);
        ArgumentNullException.ThrowIfNull(body);

        RecordId = recordId;
        PeriodId = periodId;
        Body = body;
    }
}

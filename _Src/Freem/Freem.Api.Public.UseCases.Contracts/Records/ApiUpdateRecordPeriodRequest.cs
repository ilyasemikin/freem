using Freem.Domain.Records;
using Freem.Api.Public.Contracts.DTO.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiUpdateRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.UpdatePeriod;

    public RecordId RecordId { get; }
    public RecordPeriodId PeriodId { get; }
    public UpdateRecordPeriodRequest Body { get; }

    public ApiUpdateRecordPeriodRequest(RecordId recordId, RecordPeriodId periodId, UpdateRecordPeriodRequest body)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(periodId);
        ArgumentNullException.ThrowIfNull(body);

        RecordId = recordId;
        PeriodId = periodId;
        Body = body;
    }
}

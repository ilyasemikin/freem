using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Time.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.AddPeriod;

public sealed class AddRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.AddPeriod;

    public RecordId RecordId { get; }
    public DateTimePeriod Period { get; }

    public RecordPeriodDescription? Description { get; init; }

    public AddRecordPeriodRequest(RecordId recordId, DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(period);

        RecordId = recordId;
        Period = period;
    }
}

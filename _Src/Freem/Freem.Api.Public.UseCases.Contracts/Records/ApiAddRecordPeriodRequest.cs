using Freem.Domain.Records;
using Freem.Api.Public.Contracts.DTO.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiAddRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.AddPeriod;

    public RecordId RecordId { get; }
    public RecordPeriodRequest Body { get; }

    public ApiAddRecordPeriodRequest(RecordId recordId, RecordPeriodRequest body)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(body);

        RecordId = recordId;
        Body = body;
    }
}

using Freem.Api.Public.Contracts.V1.Records;
using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiAddRecordPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.AddPeriod;

    public RecordId RecordId { get; }
    public RecordPeriodItem Body { get; }

    public ApiAddRecordPeriodRequest(RecordId recordId, RecordPeriodItem body)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(body);

        RecordId = recordId;
        Body = body;
    }
}

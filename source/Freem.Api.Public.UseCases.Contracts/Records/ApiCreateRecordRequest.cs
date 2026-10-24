using Freem.Api.Public.Contracts.V1.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiCreateRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Create;

    public RecordCreateRequest Body { get; }

    public ApiCreateRecordRequest(RecordCreateRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}

using Freem.Api.Public.Contracts.V1.Records;
using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiUpdateRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Update;

    public RecordId Id { get; }
    public RecordUpdateRequest Body { get; }

    public ApiUpdateRecordRequest(RecordId id, RecordUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}

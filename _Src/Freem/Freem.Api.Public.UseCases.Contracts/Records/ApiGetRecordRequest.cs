using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiGetRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Get;

    public RecordId Id { get; }

    public ApiGetRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}

using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.Get;

public sealed class GetRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Get;

    public RecordId Id { get; }

    public GetRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}
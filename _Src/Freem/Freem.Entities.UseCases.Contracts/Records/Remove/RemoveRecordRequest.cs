using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Records.Remove;

public sealed class RemoveRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Remove;

    public RecordId Id { get; }

    public RemoveRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}
using Freem.Domain.Records;

namespace Freem.Entities.UseCases.Contracts.Records.Get;

public sealed class GetRecordRequest
{
    public RecordId Id { get; }

    public GetRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}